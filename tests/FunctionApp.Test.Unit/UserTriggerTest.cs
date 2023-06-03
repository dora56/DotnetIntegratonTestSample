using Core.Entities;
using FunctionApp.Models;

namespace FunctionApp.Test.Unit;

public class UserTriggerTest
{
    private Mock<IUserRepository> _userRepositoryMock = new();
    private Mock<ILogger<UserTrigger>> _loggerMock = new();
    
    [Fact]
    public async Task Success_UserCreateRunQueueAsync()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        var sut = new UserTrigger(_userRepositoryMock.Object, _loggerMock.Object);

        // Act
        await sut.UserCreateRunQueueAsync(new UserQueueItem("testuser"));

        // Assert
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
