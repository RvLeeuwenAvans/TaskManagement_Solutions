using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.DTO.Office.Relation;
using TaskManagement.Test.ServiceTests.Helpers;

namespace TaskManagement.Test.ServiceTests;

public class RelationServiceTests
{
    private readonly Mock<IRelationRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<RelationCreateDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<RelationUpdateDto>> _updateValidatorMock = new();
    private readonly RelationService _service;

    public RelationServiceTests()
    {
        _service = new RelationService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetRelationsByOffice Tests

    [Fact]
    public async Task GetRelationsByOffice_ReturnsMappedDtos()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var relations = new List<Relation>
        {
            TestHelpers.CreateTestRelation(firstName: "John", lastName: "Doe"),
            TestHelpers.CreateTestRelation(firstName: "Jane", lastName: "Smith")
        };

        var dtos = relations.Select(r => new RelationResponseDto
        {
            Id = r.Id,
            FirstName = r.FirstName,
            LastName = r.LastName,
            OfficeId = r.OfficeId,
            RelationNumber = r.RelationNumber
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(relations.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<RelationResponseDto>>(It.Is<List<Relation>>(l =>
                l.All(r => r.OfficeId == officeId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetRelationsByOffice(officeId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<RelationResponseDto>>(It.Is<List<Relation>>(l =>
            l.All(r => r.OfficeId == officeId))), Times.Once);
    }

    [Fact]
    public async Task GetRelationsByOffice_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var relations = new List<Relation>();
        var dtos = new List<RelationResponseDto>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(relations.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<RelationResponseDto>>(relations))
            .Returns(dtos);

        // Act
        var result = await _service.GetRelationsByOffice(officeId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<RelationResponseDto>>(relations), Times.Once);
    }

    #endregion

    #region GetRelationByIdAsync Tests

    [Fact]
    public async Task GetRelationByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var relation = TestHelpers.CreateTestRelation();
        var dto = new RelationResponseDto
        {
            Id = relation.Id,
            FirstName = relation.FirstName,
            LastName = relation.LastName,
            OfficeId = relation.OfficeId,
            RelationNumber = relation.RelationNumber
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(relation.Id))
            .ReturnsAsync(relation);

        _mapperMock
            .Setup(m => m.Map<RelationResponseDto>(relation))
            .Returns(dto);

        // Act
        var result = await _service.GetRelationByIdAsync(relation.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(relation.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<RelationResponseDto>(relation), Times.Once);
    }

    [Fact]
    public async Task GetRelationByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Relation?)null);

        // Act
        var result = await _service.GetRelationByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<RelationResponseDto>(It.IsAny<Relation>()), Times.Never);
    }

    #endregion

    #region CreateRelationAsync Tests

    [Theory]
    [InlineData("John", "Doe")]
    [InlineData("Jane", "Smith")]
    [InlineData("Bob", "Johnson")]
    public async Task CreateRelationAsync_ValidDto_ReturnsMappedDto(string firstName, string lastName)
    {
        // Arrange
        var dto = new RelationCreateDto
        {
            FirstName = firstName,
            LastName = lastName,
            OfficeId = Guid.NewGuid()
        };

        var relation = TestHelpers.CreateTestRelation(
            firstName: firstName,
            lastName: lastName);

        var response = new RelationResponseDto
        {
            Id = relation.Id,
            FirstName = relation.FirstName,
            LastName = relation.LastName,
            OfficeId = relation.OfficeId,
            RelationNumber = relation.RelationNumber
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<Relation>(dto))
            .Returns(relation);

        _mapperMock
            .Setup(m => m.Map<RelationResponseDto>(relation))
            .Returns(response);

        // Act
        var result = await _service.CreateRelationAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Relation>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(relation), Times.Once);
        _mapperMock.Verify(m => m.Map<RelationResponseDto>(relation), Times.Once);
    }

    [Fact]
    public async Task CreateRelationAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new RelationCreateDto { FirstName = "", LastName = "" };
        var failures = new List<ValidationFailure>
        {
            new("FirstName", "FirstName is required"),
            new("LastName", "LastName is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateRelationAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Relation>(It.IsAny<RelationCreateDto>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Relation>()), Times.Never);
    }

    #endregion

    #region UpdateRelationAsync Tests

    [Fact]
    public async Task UpdateRelationAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new RelationUpdateDto
        {
            Id = id,
            FirstName = "Updated",
            LastName = "Person"
        };

        var existing = TestHelpers.CreateTestRelation(id: id);

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateRelationAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateRelationAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new RelationUpdateDto { Id = id, FirstName = "Test", LastName = "User" };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Relation?)null);

        // Act
        var result = await _service.UpdateRelationAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<RelationUpdateDto>(), It.IsAny<Relation>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Relation>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRelationAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new RelationUpdateDto { Id = Guid.NewGuid(), FirstName = "", LastName = "" };
        var failures = new List<ValidationFailure>
        {
            new("FirstName", "FirstName cannot be empty"),
            new("LastName", "LastName cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateRelationAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<RelationUpdateDto>(), It.IsAny<Relation>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Relation>()), Times.Never);
    }

    #endregion

    #region DeleteRelationAsync Tests

    [Fact]
    public async Task DeleteRelationAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var relation = TestHelpers.CreateTestRelation();

        _repoMock
            .Setup(r => r.GetByIdAsync(relation.Id))
            .ReturnsAsync(relation);

        // Act
        var result = await _service.DeleteRelationAsync(relation.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(relation.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(relation.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteRelationAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Relation?)null);

        // Act
        var result = await _service.DeleteRelationAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}