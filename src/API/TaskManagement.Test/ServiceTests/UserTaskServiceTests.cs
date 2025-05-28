using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.Test.ServiceTests.Helpers;

namespace TaskManagement.Test.ServiceTests;

public class UserTaskServiceTests
{
    private readonly Mock<IUserTaskRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateUserTask>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateUserTask>> _updateValidatorMock = new();
    private readonly UserTaskService _service;

    public UserTaskServiceTests()
    {
        _service = new UserTaskService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetTasksByUser Tests

    [Fact]
    public async Task GetTasksByUser_ReturnsMappedDtos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<UserTask>
        {
            TestHelpers.CreateTestUserTask(description: "Task 1", userId: userId),
            TestHelpers.CreateTestUserTask(description: "Task 2", userId: userId)
        };

        var dtos = tasks.Select(t => new UserTaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            User = new UserResponse
            {
                Id = t.User.Id,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                Email = t.User.Email,
            },
            DueDate = t.DueDate,
            CreatorName = t.CreatorName
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(tasks.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserTaskResponse>>(It.Is<List<UserTask>>(l =>
                l.All(t => t.UserId == userId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetTasksByUser(userId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserTaskResponse>>(It.Is<List<UserTask>>(l =>
            l.All(t => t.UserId == userId))), Times.Once);
    }

    [Fact]
    public async Task GetTasksByUser_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<UserTask>();
        var dtos = new List<UserTaskResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(tasks.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<UserTaskResponse>>(tasks))
            .Returns(dtos);

        // Act
        var result = await _service.GetTasksByUser(userId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<UserTaskResponse>>(tasks), Times.Once);
    }

    #endregion

    #region GetTaskByIdAsync Tests

    [Fact]
    public async Task GetTaskByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var task = TestHelpers.CreateTestUserTask();
        var dto = new UserTaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            User = new UserResponse
            {
                Id = task.User.Id,
                FirstName = task.User.FirstName,
                LastName = task.User.LastName,
                Email = task.User.Email,
            },
            DueDate = task.DueDate,
            CreatorName = task.CreatorName
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        _mapperMock
            .Setup(m => m.Map<UserTaskResponse>(task))
            .Returns(dto);

        // Act
        var result = await _service.GetTaskByIdAsync(task.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(task.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<UserTaskResponse>(task), Times.Once);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((UserTask?)null);

        // Act
        var result = await _service.GetTaskByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<UserTaskResponse>(It.IsAny<UserTask>()), Times.Never);
    }

    #endregion

    #region CreateTaskAsync Tests

    [Theory]
    [InlineData("New Task 1", "Description 1")]
    [InlineData("New Task 2", "Description 2")]
    [InlineData("New Task 3", null)]
    public async Task CreateTaskAsync_ValidDto_ReturnsMappedDto(string title, string? description)
    {
        // Arrange
        var dto = new CreateUserTask
        {
            Title = title,
            Description = description,
            UserId = Guid.NewGuid(),
            DueDate = DateTime.Now.AddDays(1),
            CreatorName = "Henk de maker van alles"
        };

        var task = TestHelpers.CreateTestUserTask(title: title, description: description, userId: dto.UserId);
        var response = new UserTaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            User = new UserResponse
            {
                Id = task.User.Id,
                FirstName = task.User.FirstName,
                LastName = task.User.LastName,
                Email = task.User.Email,
            },
            DueDate = task.DueDate,
            CreatorName = task.CreatorName
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<UserTask>(dto))
            .Returns(task);

        _mapperMock
            .Setup(m => m.Map<UserTaskResponse>(task))
            .Returns(response);

        // Act
        var result = await _service.CreateTaskAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserTask>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(task), Times.Once);
        _mapperMock.Verify(m => m.Map<UserTaskResponse>(task), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateUserTask
        {
            Title = "",
            UserId = Guid.NewGuid(),
            DueDate = DateTime.Now.AddDays(1),
            CreatorName = "Henk de maker van alles"
        };

        var failures = new List<ValidationFailure>
        {
            new("Title", "Title is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateTaskAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserTask>(It.IsAny<CreateUserTask>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<UserTask>()), Times.Never);
    }

    #endregion

    #region UpdateTaskAsync Tests

    [Fact]
    public async Task UpdateTaskAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateUserTask
        {
            Id = id,
            Title = "Updated Task",
            Description = "Updated Description"
        };

        var existing = TestHelpers.CreateTestUserTask(Guid.NewGuid(), "Old Task", "Old Description");

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateTaskAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateUserTask { Id = id, Title = "Updated Task" };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((UserTask?)null);

        // Act
        var result = await _service.UpdateTaskAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateUserTask>(), It.IsAny<UserTask>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<UserTask>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTaskAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateUserTask { Id = Guid.NewGuid(), Title = "" };
        var failures = new List<ValidationFailure>
        {
            new("Title", "Title cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateTaskAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateUserTask>(), It.IsAny<UserTask>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<UserTask>()), Times.Never);
    }

    #endregion

    #region DeleteTaskAsync Tests

    [Fact]
    public async Task DeleteTaskAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var task = TestHelpers.CreateTestUserTask();

        _repoMock
            .Setup(r => r.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        // Act
        var result = await _service.DeleteTaskAsync(task.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(task.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(task.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((UserTask?)null);

        // Act
        var result = await _service.DeleteTaskAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}