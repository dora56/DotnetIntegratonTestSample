using System;
using System.Data.Common;

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
    
}
