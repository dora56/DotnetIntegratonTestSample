namespace FunctionApp;

public class UserTrigger
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserTrigger> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public UserTrigger(IUserRepository userRepository, ILogger<UserTrigger> logger, BlobServiceClient blobServiceClient)
    {
        _userRepository = userRepository;
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }
    
    [FunctionName(nameof(CreateUserRunQueueAsync))]
    public async Task CreateUserRunQueueAsync(
        [QueueTrigger("functest-user-create")] UserQueueItem item)
    {
        var user = new User(item.Name);
        await _userRepository.AddAsync(user);
    }
    
    [FunctionName(nameof(UpdateUserRunQueueAsync))]
    public async Task UpdateUserRunQueueAsync(
        [QueueTrigger("functest-user-update")] UserQueueItem item)
    {
        var user = await _userRepository.GetByIdAsync(Guid.Parse(item.Id));
        if (user == null)
        {
            _logger.LogWarning("User with id {Id} not found", item.Id.ToString());
            return;
        }

        user.Name = item.Name;
        await _userRepository.UpdateAsync(user);
    }
    
    [FunctionName(nameof(UserQueueToBlobAsync))]
    public async Task UserQueueToBlobAsync(
        [QueueTrigger("functest-user-file")] UserQueueItem item,
        string name)
    {
        // item to json
        var json = JsonSerializer.Serialize(item);
        // json to stream
        await using var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
        await writer.FlushAsync();
        stream.Position = 0;
        
        // upload stream to blob
        var containerClient = _blobServiceClient.GetBlobContainerClient("functest-user-file");
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(name);
        await blobClient.UploadAsync(stream, true);
    }
    
}
