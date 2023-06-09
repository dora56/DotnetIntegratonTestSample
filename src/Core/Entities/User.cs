namespace Core.Entities;

public class User
{
    public Guid Id { get; }
    
    public string Name { get; set; }
    
    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
