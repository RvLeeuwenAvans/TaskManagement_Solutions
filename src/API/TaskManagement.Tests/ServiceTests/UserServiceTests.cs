using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;
using TaskManagement.Tests.ServiceTests.Helpers;

namespace TaskManagement.Tests.ServiceTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateUser>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateUser>> _updateValidatorMock = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _passwordHasherMock.Object);
    }

    #region GetUsersByOffice Tests

    [Fact]
    public async Task GetUsersByOffice_ReturnsMappedDtos()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var users = new List<User>
        {
            TestHelpers.CreateTestUser(firstName: "John", lastName: "Smith"),
            TestHelpers.CreateTestUser(firstName: "Jane", lastName: "Doe")
        };

        var dtos = users.Select(u => new UserResponse
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            OfficeId = u.OfficeId,
            Email = u.Email
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(users.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserResponse>>(It.Is<List<User>>(l =>
                l.All(u => u.OfficeId == officeId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetUsersFromOffice(officeId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserResponse>>(It.Is<List<User>>(l =>
            l.All(u => u.OfficeId == officeId))), Times.Once);
    }

    [Fact]
    public async Task GetUsersByOffice_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var users = new List<User>();
        var dtos = new List<UserResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(users.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserResponse>>(users))
            .Returns(dtos);

        // Act
        var result = await _service.GetUsersFromOffice(officeId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserResponse>>(users), Times.Once);
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser();
        var dto = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            OfficeId = user.OfficeId,
            Email = user.Email
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(m => m.Map<UserResponse>(user))
            .Returns(dto);

        // Act
        var result = await _service.GetUserByIdAsync(user.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponse>(user), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetUserByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponse>(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region CreateUserAsync Tests

    [Theory]
    [InlineData("John", "Smith")]
    [InlineData("Jane", "Doe")]
    [InlineData("Bob", "Johnson")]
    public async Task CreateUserAsync_ValidDto_ReturnsMappedDto(string firstName, string lastName)
    {
        // Arrange
        var dto = new CreateUser
        {
            FirstName = firstName,
            LastName = lastName,
            OfficeId = Guid.NewGuid(),
            Password = "<PASSWORD>",
            Email = $"{firstName.ToLower()}.{lastName.ToLower()}@test.com"
        };

        var user = TestHelpers.CreateTestUser(
            firstName: firstName,
            lastName: lastName);

        var hashedPassword = "hashedPassword123";

        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            OfficeId = user.OfficeId,
            Email = user.Email
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<User>(dto))
            .Returns(user);

        _passwordHasherMock
            .Setup(h => h.HashPassword(user, dto.Password))
            .Returns(hashedPassword);

        _mapperMock
            .Setup(m => m.Map<UserResponse>(user))
            .Returns(response);

        // Act
        var result = await _service.CreateUserAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<User>(dto), Times.Once);
        _passwordHasherMock.Verify(h => h.HashPassword(user, dto.Password), Times.Once);
        user.Password.Should().Be(hashedPassword);
        _repoMock.Verify(r => r.AddAsync(user), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponse>(user), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateUser
        {
            FirstName = "",
            LastName = "",
            Password = "<Password>",
            Email = "test@email.com"
        };
        var failures = new List<ValidationFailure>
        {
            new("FirstName", "FirstName is required"),
            new("LastName", "LastName is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateUserAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<CreateUser>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region UpdateUserAsync Tests

    [Fact]
    public async Task UpdateUserAsync_WithPassword_HashesNewPassword()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateUser
        {
            Id = id,
            FirstName = "Updated",
            LastName = "Person",
            Password = "newPassword123",
            Email = "updated.person@example.com"
        };

        var existing = TestHelpers.CreateTestUser(id: id);
        var hashedPassword = "hashedNewPassword456";

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _passwordHasherMock
            .Setup(h => h.HashPassword(existing, dto.Password))
            .Returns(hashedPassword);

        // Act
        var result = await _service.UpdateUserAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _passwordHasherMock.Verify(h => h.HashPassword(existing, dto.Password), Times.Once);
        existing.Password.Should().Be(hashedPassword);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateUser { Id = id, FirstName = "Test", LastName = "User" };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.UpdateUserAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateUser>(), It.IsAny<User>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateUser { Id = Guid.NewGuid(), FirstName = "", LastName = "" };
        var failures = new List<ValidationFailure>
        {
            new("FirstName", "FirstName cannot be empty"),
            new("LastName", "LastName cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateUserAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateUser>(), It.IsAny<User>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region DeleteUserAsync Tests

    [Fact]
    public async Task DeleteUserAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser();

        _repoMock
            .Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        // Act
        var result = await _service.DeleteUserAsync(user.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(user.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.DeleteUserAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}