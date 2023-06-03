namespace Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        await _context.Users.AddAsync(user);

        await SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        
        _context.Users.Update(user);

        await SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        
        _context.Users.Remove(user);
        
        await SaveChangesAsync();
    }
    
    private async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
