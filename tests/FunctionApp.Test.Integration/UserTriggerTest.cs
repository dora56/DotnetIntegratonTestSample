namespace FunctionApp.Test;

[Collection(IntegrationTestsCollection.Name)]
public class UserTriggerTest: IClassFixture<TestStartup>, IAsyncLifetime
{
    private readonly UserTrigger _trigger;
    private readonly AppDbContext _dbContext;
    private User? _user;

    public UserTriggerTest(
        TestsInitializer initializer)
    {
        _trigger = initializer.ServiceProvider.GetRequiredService<UserTrigger>();
        _dbContext = initializer.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();
    }

    [Fact]
    public async void Success_CreateUser()
    {
        // Arrange
        var item = new UserQueueItem("testuser");
        
        // Act
        await _trigger.UserCreateRunQueueAsync(item);
        _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == item.Name);
        
        // Assert
        _user.Should().NotBeNull("because we created a user");
        _user?.Name.Should().Be("testuser", "because we created a user with the name 'testuser'");
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
