using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id);
    
    public Task AddAsync(User user);
    
    public Task UpdateAsync(User user);
    
    public Task DeleteAsync(User user);
}
