using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;
using TaskManagement.Test.ServiceTests.Helpers;

namespace TaskManagement.Test.ServiceTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<UserCreateDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UserUpdateDto>> _updateValidatorMock = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
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

        var dtos = users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            OfficeId = u.OfficeId
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(users.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserResponseDto>>(It.Is<List<User>>(l =>
                l.All(u => u.OfficeId == officeId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetUsersFromOffice(officeId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserResponseDto>>(It.Is<List<User>>(l =>
            l.All(u => u.OfficeId == officeId))), Times.Once);
    }

    [Fact]
    public async Task GetUsersByOffice_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var users = new List<User>();
        var dtos = new List<UserResponseDto>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(users.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserResponseDto>>(users))
            .Returns(dtos);

        // Act
        var result = await _service.GetUsersFromOffice(officeId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserResponseDto>>(users), Times.Once);
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var user = TestHelpers.CreateTestUser();
        var dto = new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            OfficeId = user.OfficeId
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(m => m.Map<UserResponseDto>(user))
            .Returns(dto);

        // Act
        var result = await _service.GetUserByIdAsync(user.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponseDto>(user), Times.Once);
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
        _mapperMock.Verify(m => m.Map<UserResponseDto>(It.IsAny<User>()), Times.Never);
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
        var dto = new UserCreateDto
        {
            FirstName = firstName,
            LastName = lastName,
            OfficeId = Guid.NewGuid()
        };

        var user = TestHelpers.CreateTestUser(
            firstName: firstName,
            lastName: lastName);

        var response = new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            OfficeId = user.OfficeId
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<User>(dto))
            .Returns(user);

        _mapperMock
            .Setup(m => m.Map<UserResponseDto>(user))
            .Returns(response);

        // Act
        var result = await _service.CreateUserAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<User>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(user), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponseDto>(user), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UserCreateDto { FirstName = "", LastName = "" };
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
        _mapperMock.Verify(m => m.Map<User>(It.IsAny<UserCreateDto>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region UpdateUserAsync Tests

    [Fact]
    public async Task UpdateUserAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UserUpdateDto
        {
            Id = id,
            FirstName = "Updated",
            LastName = "Person"
        };

        var existing = TestHelpers.CreateTestUser(id: id);

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateUserAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UserUpdateDto { Id = id, FirstName = "Test", LastName = "User" };

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
        _mapperMock.Verify(m => m.Map(It.IsAny<UserUpdateDto>(), It.IsAny<User>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UserUpdateDto { Id = Guid.NewGuid(), FirstName = "", LastName = "" };
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
        _mapperMock.Verify(m => m.Map(It.IsAny<UserUpdateDto>(), It.IsAny<User>()), Times.Never);
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