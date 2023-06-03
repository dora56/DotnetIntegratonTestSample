namespace Core.Entities;

public class User
{
    public Guid Id { get; }
    
    public string Name { get; } = default!;
    
    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
