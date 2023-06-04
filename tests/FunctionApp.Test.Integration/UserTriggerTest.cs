using System.Text.Json;
using Azure.Storage.Blobs;

namespace FunctionApp.Test;

[Collection(IntegrationTestsCollection.Name)]
public class UserTriggerTest : IClassFixture<TestStartup>, IAsyncLifetime
{
    private readonly UserTrigger _trigger;
    private readonly AppDbContext _dbContext;
    private readonly BlobServiceClient _blobServiceClient;
    private  BlobContainerClient _containerClient;
    private User? _user;

    public UserTriggerTest(
        TestsInitializer initializer)
    {
        _trigger = initializer.ServiceProvider.GetRequiredService<UserTrigger>();
        _dbContext = initializer.ServiceProvider.GetRequiredService<AppDbContext>();
        _blobServiceClient = initializer.ServiceProvider.GetRequiredService<BlobServiceClient>();
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();
        
        _containerClient = _blobServiceClient.GetBlobContainerClient("functest-user-file");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async void Success_CreateUser()
    {
        // Arrange
        var item = new UserQueueItem() { Name = "testUser" };

        // Act
        await _trigger.CreateUserRunQueueAsync(item);
        _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == item.Name);

        // Assert
        _user.Should().NotBeNull("because we created a user");
        _user?.Name.Should().Be("testUser", "because we created a user with the name 'testUser'");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async void Success_UpdateUser()
    {
        // Arrange
        var testUser = new User("testUser");
        await _dbContext.Users.AddAsync(testUser);
        await _dbContext.SaveChangesAsync();
        var item = new UserQueueItem {Id = testUser.Id.ToString(), Name = "testUser2"};
        
        // Act
        await _trigger.UpdateUserRunQueueAsync(item);
        _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(item.Id));
        
        // Assert
        _user.Should().NotBeNull("because we updated a user");
        _user?.Name.Should().Be("testUser2", "because we updated a user with the name 'testUser2'");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async void Success_UserQueueToBlob()
    {
        // Arrange
        var item = new UserQueueItem {Id = Guid.NewGuid().ToString(), Name = "testUser"};
        var name = $"{item.Id}.json";

        // Act
        await _trigger.UserQueueToBlobAsync(item, name);

        // Assert
        var blobClient = _containerClient.GetBlobClient(name);
        (await blobClient.ExistsAsync()).Value.Should().BeTrue( "because we uploaded a blob");
        
        var download = await blobClient.DownloadAsync();
        using var streamReader = new StreamReader(download.Value.Content);
        var json = await streamReader.ReadToEndAsync();
        var user = JsonSerializer.Deserialize<UserQueueItem>(json);
        await blobClient.DeleteAsync();
        user.Should().NotBeNull("because we uploaded a blob");

    }

    public async Task DisposeAsync()
    {
        if (_user != null)
        {
            _dbContext.Users.Remove(_user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
