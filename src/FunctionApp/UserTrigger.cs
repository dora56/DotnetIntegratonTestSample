namespace FunctionApp;

public class UserTrigger
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserTrigger> _logger;

    public UserTrigger(IUserRepository userRepository, ILogger<UserTrigger> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    [FunctionName(nameof(UserCreateRunQueueAsync))]
    public async Task UserCreateRunQueueAsync(
        [QueueTrigger("functest-user-create")] UserQueueItem item)
    {
        var user = new User(item.Name);
        await _userRepository.AddAsync(user);
    }
    
}
