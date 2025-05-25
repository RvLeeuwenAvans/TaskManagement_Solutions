using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.Test.ServiceTests.Helpers;

namespace TaskManagement.Test.ServiceTests;

public class DamageClaimServiceTests
{
    private readonly Mock<IDamageClaimRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateDamageClaim>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateDamageClaim>> _updateValidatorMock = new();
    private readonly DamageClaimService _service;

    public DamageClaimServiceTests()
    {
        _service = new DamageClaimService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetAllDamageClaimsAsync Tests

    [Fact]
    public async Task GetDamageClaimsByOffice_ReturnsMappedDtos()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var claims = new List<DamageClaim>
        {
            TestHelpers.CreateTestDamageClaim(
                type: "Water Damage",
                relation: TestHelpers.CreateTestRelation()
            ),
            TestHelpers.CreateTestDamageClaim(
                type: "Fire Damage",
                relation: TestHelpers.CreateTestRelation()
            )
        };

        var dtos = claims.Select(c => new DamageClaimResponse
        {
            Id = c.Id,
            Type = c.Type,
            RelationId = c.RelationId,
            DamageNumber = c.DamageNumber,
            DamageNumberSub = c.DamageNumberSub
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(claims.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<DamageClaimResponse>>(It.Is<List<DamageClaim>>(l =>
                l.All(c => c.Relation.OfficeId == officeId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetDamageClaimsByOffice(officeId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(dtos);

        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<DamageClaimResponse>>(
                It.Is<List<DamageClaim>>(l => l.All(c => c.Relation.OfficeId == officeId))),
            Times.Once);
    }

    [Fact]
    public async Task GetDamageClaimsByOffice_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var claims = new List<DamageClaim>();
        var dtos = new List<DamageClaimResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(claims.AsQueryable());
    
        _mapperMock
            .Setup(m => m.Map<List<DamageClaimResponse>>(claims))
            .Returns(dtos);

        // Act
        var result = await _service.GetDamageClaimsByOffice(officeId);

        // Assert
        result.Should().BeEmpty();
    
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<DamageClaimResponse>>(claims), Times.Once);
    }

    #endregion

    #region GetDamageClaimByIdAsync Tests

    [Fact]
    public async Task GetDamageClaimByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var claim = TestHelpers.CreateTestDamageClaim(type: "Fire");

        var dto = new DamageClaimResponse
        {
            Id = claim.Id,
            Type = claim.Type,
            RelationId = claim.RelationId,
            DamageNumber = claim.DamageNumber,
            DamageNumberSub = claim.DamageNumberSub
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(claim.Id))
            .ReturnsAsync(claim);

        _mapperMock
            .Setup(m => m.Map<DamageClaimResponse>(claim))
            .Returns(dto);

        // Act
        var result = await _service.GetDamageClaimByIdAsync(claim.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dto);

        _repoMock.Verify(r => r.GetByIdAsync(claim.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<DamageClaimResponse>(claim), Times.Once);
    }

    [Fact]
    public async Task GetDamageClaimByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((DamageClaim?)null);

        // Act
        var result = await _service.GetDamageClaimByIdAsync(id);

        // Assert
        result.Should().BeNull();

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<DamageClaimResponse>(It.IsAny<DamageClaim>()), Times.Never);
    }

    #endregion

    #region CreateDamageClaimAsync Tests

    [Theory]
    [InlineData("Storm")]
    [InlineData("Flood")]
    [InlineData("Earthquake")]
    public async Task CreateDamageClaimAsync_ValidDto_ReturnsMappedDto(string damageType)
    {
        // Arrange
        var relationId = Guid.NewGuid();
        var dto = new CreateDamageClaim
        {
            RelationId = relationId,
            Type = damageType
        };

        var claim = TestHelpers.CreateTestDamageClaim(
            relation: TestHelpers.CreateTestRelation(relationId),
            type: damageType);

        var response = new DamageClaimResponse
        {
            Id = claim.Id,
            RelationId = claim.RelationId,
            Type = claim.Type,
            DamageNumber = claim.DamageNumber,
            DamageNumberSub = claim.DamageNumberSub
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<DamageClaim>(dto))
            .Returns(claim);

        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<DamageClaim>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<DamageClaimResponse>(It.IsAny<DamageClaim>()))
            .Returns(response);

        // Act
        var result = await _service.CreateDamageClaimAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(response);
        result.Type.Should().Be(damageType);

        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<DamageClaim>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<DamageClaim>()), Times.Once);
        _mapperMock.Verify(m => m.Map<DamageClaimResponse>(It.IsAny<DamageClaim>()), Times.Once);
    }

    [Fact]
    public async Task CreateDamageClaimAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateDamageClaim
        {
            RelationId = Guid.NewGuid(),
            Type = "" // Empty type will fail validation
        };

        var failures = new List<ValidationFailure>
        {
            new("Type", "Type is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateDamageClaimAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<DamageClaim>(It.IsAny<CreateDamageClaim>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<DamageClaim>()), Times.Never);
    }

    #endregion

    #region UpdateDamageClaimAsync Tests

    [Fact]
    public async Task UpdateDamageClaimAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateDamageClaim
        {
            Id = id,
            Type = "Updated Type"
        };

        var existing = TestHelpers.CreateTestDamageClaim(id: id, type: "Old Type");

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _mapperMock
            .Setup(m => m.Map(dto, existing));

        _repoMock
            .Setup(r => r.UpdateAsync(existing))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateDamageClaimAsync(dto);

        // Assert
        result.Should().BeTrue();

        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateDamageClaimAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateDamageClaim
        {
            Id = id,
            Type = "Something"
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((DamageClaim?)null);

        // Act
        var result = await _service.UpdateDamageClaimAsync(dto);

        // Assert
        result.Should().BeFalse();

        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateDamageClaim>(), It.IsAny<DamageClaim>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<DamageClaim>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDamageClaimAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateDamageClaim
        {
            Id = Guid.NewGuid(),
            Type = "" // Invalid empty type
        };

        var failures = new List<ValidationFailure>
        {
            new("Type", "Type cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateDamageClaimAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateDamageClaim>(), It.IsAny<DamageClaim>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<DamageClaim>()), Times.Never);
    }

    #endregion

    #region DeleteDamageClaimAsync Tests

    [Fact]
    public async Task DeleteDamageClaimAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var claim = TestHelpers.CreateTestDamageClaim();

        _repoMock
            .Setup(r => r.GetByIdAsync(claim.Id))
            .ReturnsAsync(claim);

        _repoMock
            .Setup(r => r.DeleteAsync(claim.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteDamageClaimAsync(claim.Id);

        // Assert
        result.Should().BeTrue();

        _repoMock.Verify(r => r.GetByIdAsync(claim.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(claim.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteDamageClaimAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((DamageClaim?)null);

        // Act
        var result = await _service.DeleteDamageClaimAsync(id);

        // Assert
        result.Should().BeFalse();

        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}