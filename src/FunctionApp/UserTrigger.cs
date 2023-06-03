namespace FunctionApp;

public class UserTrigger
{
    private readonly IUserRepository _userRepository;
    
    public UserTrigger(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [FunctionName("UserCreate")]
    public async Task RunAsync(
        [QueueTrigger("functest-user-create")] UserQueueItem item,
        ILogger log)
    {
        var user = new User(item.Name);
        await _userRepository.AddAsync(user);
    }
    
}
