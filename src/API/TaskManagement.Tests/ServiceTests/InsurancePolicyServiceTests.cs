using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;
using TaskManagement.Tests.ServiceTests.Helpers;

namespace TaskManagement.Tests.ServiceTests;

public class InsurancePolicyServiceTests
{
    private readonly Mock<IInsurancePolicyRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateInsurancePolicy>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateInsurancePolicy>> _updateValidatorMock = new();
    private readonly InsurancePolicyService _service;

    public InsurancePolicyServiceTests()
    {
        _service = new InsurancePolicyService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetAllInsurancePoliciesAsync Tests

    [Fact]
    public async Task GetInsurancePoliciesByOffice_ReturnsMappedDtos()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var policies = new List<InsurancePolicy>
        {
            TestHelpers.CreateTestInsurancePolicy(
                type: "Car Insurance",
                relation: TestHelpers.CreateTestRelation()
            ),
            TestHelpers.CreateTestInsurancePolicy(
                type: "Home Insurance",
                relation: TestHelpers.CreateTestRelation()
            )
        };

        var dtos = policies.Select(p => new InsurancePolicyResponse
        {
            Id = p.Id,
            Type = p.Type,
            RelationId = p.RelationId,
            PolicyNumber = p.PolicyNumber
        }).ToList();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(policies.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<InsurancePolicyResponse>>(It.Is<List<InsurancePolicy>>(l =>
                l.All(p => p.Relation.OfficeId == officeId))))
            .Returns(dtos);

        // Act
        var result = await _service.GetInsurancePoliciesByOffice(officeId);

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<InsurancePolicyResponse>>(It.Is<List<InsurancePolicy>>(l =>
            l.All(p => p.Relation.OfficeId == officeId))), Times.Once);
    }

    [Fact]
    public async Task GetInsurancePoliciesByOffice_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var policies = new List<InsurancePolicy>().AsQueryable();
        var dtos = new List<InsurancePolicyResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(policies);

        _mapperMock
            .Setup(m => m.Map<List<InsurancePolicyResponse>>(It.Is<List<InsurancePolicy>>(l => !l.Any())))
            .Returns(dtos);

        // Act
        var result = await _service.GetInsurancePoliciesByOffice(officeId);

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<InsurancePolicyResponse>>(It.Is<List<InsurancePolicy>>(l => !l.Any())),
            Times.Once);
    }

    #endregion

    #region GetInsurancePolicyByIdAsync Tests

    [Fact]
    public async Task GetInsurancePolicyByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var policy = TestHelpers.CreateTestInsurancePolicy();
        var dto = new InsurancePolicyResponse
        {
            Id = policy.Id,
            Type = policy.Type,
            RelationId = policy.RelationId,
            PolicyNumber = policy.PolicyNumber
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        _mapperMock
            .Setup(m => m.Map<InsurancePolicyResponse>(policy))
            .Returns(dto);

        // Act
        var result = await _service.GetInsurancePolicyByIdAsync(policy.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(policy.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<InsurancePolicyResponse>(policy), Times.Once);
    }

    [Fact]
    public async Task GetInsurancePolicyByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((InsurancePolicy?)null);

        // Act
        var result = await _service.GetInsurancePolicyByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<InsurancePolicyResponse>(It.IsAny<InsurancePolicy>()), Times.Never);
    }

    #endregion

    #region CreateInsurancePolicyAsync Tests

    [Theory]
    [InlineData("Car Insurance")]
    [InlineData("Home Insurance")]
    [InlineData("Life Insurance")]
    public async Task CreateInsurancePolicyAsync_ValidDto_ReturnsMappedDto(string policyType)
    {
        // Arrange
        var relationId = Guid.NewGuid();
        var dto = new CreateInsurancePolicy
        {
            RelationId = relationId,
            Type = policyType
        };

        var policy = TestHelpers.CreateTestInsurancePolicy(
            relation: TestHelpers.CreateTestRelation(relationId),
            type: policyType);

        var response = new InsurancePolicyResponse
        {
            Id = policy.Id,
            RelationId = policy.RelationId,
            Type = policy.Type,
            PolicyNumber = policy.PolicyNumber
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<InsurancePolicy>(dto))
            .Returns(policy);

        _mapperMock
            .Setup(m => m.Map<InsurancePolicyResponse>(It.IsAny<InsurancePolicy>()))
            .Returns(response);

        // Act
        var result = await _service.CreateInsurancePolicyAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<InsurancePolicy>(dto), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<InsurancePolicy>()), Times.Once);
        _mapperMock.Verify(m => m.Map<InsurancePolicyResponse>(It.IsAny<InsurancePolicy>()), Times.Once);
    }

    [Fact]
    public async Task CreateInsurancePolicyAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateInsurancePolicy
        {
            RelationId = Guid.NewGuid(),
            Type = string.Empty
        };

        var failures = new List<ValidationFailure>
        {
            new("Type", "Type is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateInsurancePolicyAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<InsurancePolicy>(It.IsAny<CreateInsurancePolicy>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<InsurancePolicy>()), Times.Never);
    }

    #endregion

    #region UpdateInsurancePolicyAsync Tests

    [Fact]
    public async Task UpdateInsurancePolicyAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateInsurancePolicy
        {
            Id = id,
            Type = "Updated Insurance"
        };

        var existing = TestHelpers.CreateTestInsurancePolicy(id: id, type: "Old Insurance");

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _mapperMock
            .Setup(m => m.Map(dto, existing));

        // Act
        var result = await _service.UpdateInsurancePolicyAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateInsurancePolicyAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateInsurancePolicy
        {
            Id = id,
            Type = "Something"
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((InsurancePolicy?)null);

        // Act
        var result = await _service.UpdateInsurancePolicyAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateInsurancePolicy>(), It.IsAny<InsurancePolicy>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<InsurancePolicy>()), Times.Never);
    }

    [Fact]
    public async Task UpdateInsurancePolicyAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateInsurancePolicy
        {
            Id = Guid.NewGuid(),
            Type = string.Empty
        };

        var failures = new List<ValidationFailure>
        {
            new("Type", "Type cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateInsurancePolicyAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateInsurancePolicy>(), It.IsAny<InsurancePolicy>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<InsurancePolicy>()), Times.Never);
    }

    #endregion

    #region DeleteInsurancePolicyAsync Tests

    [Fact]
    public async Task DeleteInsurancePolicyAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var policy = TestHelpers.CreateTestInsurancePolicy();

        _repoMock
            .Setup(r => r.GetByIdAsync(policy.Id))
            .ReturnsAsync(policy);

        // Act
        var result = await _service.DeleteInsurancePolicyAsync(policy.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(policy.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(policy.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteInsurancePolicyAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((InsurancePolicy?)null);

        // Act
        var result = await _service.DeleteInsurancePolicyAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}