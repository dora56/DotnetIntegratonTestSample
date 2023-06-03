using System.Data.Common;
using System.Data.SqlTypes;
using Core.Entities;
using FunctionApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FunctionApp.Test.Unit;

public class UserTriggerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ILogger<UserTrigger>> _loggerMock = new();
    
    [Fact]
    public async Task Success_CreateUserRunQueueAsync()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        var sut = new UserTrigger(_userRepositoryMock.Object, _loggerMock.Object);

        // Act
        await sut.CreateUserRunQueueAsync(new UserQueueItem {Name = "testUser"});

        // Assert
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Success_UpdateUserRunQueueAsync()
    {
        // Arrange
        var testUser = new User("testUser");
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testUser);
        
        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        
        var sut = new UserTrigger(_userRepositoryMock.Object, _loggerMock.Object);
        
        // Act
        await sut.UpdateUserRunQueueAsync(new UserQueueItem {Id = testUser.Id.ToString(), Name = "testUser2"});
        
        // Assert
        _userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }
    
    [Fact]
    public async Task Success_UpdateUserRunQueueAsync_UserNotFound()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User) null);
        
        var sut = new UserTrigger(_userRepositoryMock.Object, _loggerMock.Object);
        
        // Act
        await sut.UpdateUserRunQueueAsync(new UserQueueItem {Id = Guid.NewGuid().ToString(), Name = "testUser2"});
        
        // Assert
        _userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}
