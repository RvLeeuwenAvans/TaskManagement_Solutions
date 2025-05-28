using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.User.Task.Note;
using TaskManagement.DTO.Office.User.Task.Note;
using TaskManagement.Test.ServiceTests.Helpers;

namespace TaskManagement.Test.ServiceTests;

public class NoteServiceTests
{
    private readonly Mock<INoteRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateNote>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateNote>> _updateValidatorMock = new();
    private readonly NoteService _service;

    public NoteServiceTests()
    {
        _service = new NoteService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetNotesByTask Tests

    [Fact]
    public async Task GetNotesByTask_ReturnsMappedDtos()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var notes = new List<Note>
        {
            TestHelpers.CreateTestNote(content: "Note 1", taskId: taskId),
            TestHelpers.CreateTestNote(content: "Note 1", taskId: taskId)
        };

        var dtos = notes.Select(n => new NoteResponse
        {
            Id = n.Id,
            Content = n.Content,
            TaskId = n.TaskId
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(notes.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<NoteResponse>>(It.Is<List<Note>>(l =>
                l.All(n => n.TaskId == taskId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetNotesByTask(taskId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<NoteResponse>>(It.Is<List<Note>>(l =>
            l.All(n => n.TaskId == taskId))), Times.Once);
    }

    [Fact]
    public async Task GetNotesByTask_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var notes = new List<Note>();
        var dtos = new List<NoteResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(notes.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<NoteResponse>>(notes))
            .Returns(dtos);

        // Act
        var result = await _service.GetNotesByTask(taskId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<NoteResponse>>(notes), Times.Once);
    }

    #endregion

    #region GetNoteByIdAsync Tests

    [Fact]
    public async Task GetNoteByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var note = TestHelpers.CreateTestNote(content: "Test Note");
        var dto = new NoteResponse
        {
            Id = note.Id,
            Content = note.Content,
            TaskId = note.TaskId
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(note.Id))
            .ReturnsAsync(note);

        _mapperMock
            .Setup(m => m.Map<NoteResponse>(note))
            .Returns(dto);

        // Act
        var result = await _service.GetNoteByIdAsync(note.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(note.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<NoteResponse>(note), Times.Once);
    }

    [Fact]
    public async Task GetNoteByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Note?)null);

        // Act
        var result = await _service.GetNoteByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<NoteResponse>(It.IsAny<Note>()), Times.Never);
    }

    #endregion

    #region CreateNoteAsync Tests

    [Theory]
    [InlineData("New Note 1")]
    [InlineData("New Note 2")]
    [InlineData("New Note 3")]
    public async Task CreateNoteAsync_ValidDto_ReturnsMappedDto(string content)
    {
        // Arrange
        var dto = new CreateNote { Content = content, TaskId = Guid.NewGuid() };
        var note = TestHelpers.CreateTestNote(content: dto.Content, taskId: dto.TaskId);
        var response = new NoteResponse
        {
            Id = note.Id,
            Content = note.Content,
            TaskId = note.TaskId
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<Note>(dto))
            .Returns(note);

        _mapperMock
            .Setup(m => m.Map<NoteResponse>(note))
            .Returns(response);

        // Act
        var result = await _service.CreateNoteAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Note>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(note), Times.Once);
        _mapperMock.Verify(m => m.Map<NoteResponse>(note), Times.Once);
    }

    [Fact]
    public async Task CreateNoteAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateNote { Content = "" };
        var failures = new List<ValidationFailure>
        {
            new("Content", "Content is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateNoteAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Note>(It.IsAny<CreateNote>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Note>()), Times.Never);
    }

    #endregion

    #region UpdateNoteAsync Tests

    [Fact]
    public async Task UpdateNoteAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateNote
        {
            Id = id,
            Content = "Updated Content"
        };

        var existing = TestHelpers.CreateTestNote(id: id, content: "Old Content");

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateNoteAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateNoteAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateNote { Id = id, Content = "Updated Content" };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Note?)null);

        // Act
        var result = await _service.UpdateNoteAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateNote>(), It.IsAny<Note>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Never);
    }

    [Fact]
    public async Task UpdateNoteAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateNote { Id = Guid.NewGuid(), Content = "" };
        var failures = new List<ValidationFailure>
        {
            new("Content", "Content cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateNoteAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateNote>(), It.IsAny<Note>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Never);
    }

    #endregion

    #region DeleteNoteAsync Tests

    [Fact]
    public async Task DeleteNoteAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var note = TestHelpers.CreateTestNote(content: "Test Note");
        _repoMock
            .Setup(r => r.GetByIdAsync(note.Id))
            .ReturnsAsync(note);

        // Act
        var result = await _service.DeleteNoteAsync(note.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(note.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(note.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteNoteAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Note?)null);

        // Act
        var result = await _service.DeleteNoteAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}