using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office;
using TaskManagement.DTO.Office;
using TaskManagement.Tests.ServiceTests.Helpers;

namespace TaskManagement.Tests.ServiceTests;

public class OfficeServiceTests
{
    private readonly Mock<IOfficeRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidator<CreateOffice>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateOffice>> _updateValidatorMock = new();
    private readonly OfficeService _service;

    public OfficeServiceTests()
    {
        _service = new OfficeService(
            _repoMock.Object,
            _mapperMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    #region GetAllOfficesAsync Tests

    [Fact]
    public async Task GetAllOfficesAsync_ReturnsMappedDtos()
    {
        // Arrange
        var offices = new List<Office>
        {
            TestHelpers.CreateTestOffice(name: "Office A"),
            TestHelpers.CreateTestOffice(name: "Office B")
        };

        var dtos = new List<OfficeResponse>
        {
            new() { Id = offices[0].Id, Name = offices[0].Name, OfficeCode = 1001 },
            new() { Id = offices[1].Id, Name = offices[1].Name, OfficeCode = 1002 }
        };

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(offices.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<OfficeResponse>>(offices))
            .Returns(dtos);

        // Act
        var result = await _service.GetAllOffices();

        // Assert
        result.Should().BeEquivalentTo(dtos);
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<OfficeResponse>>(offices), Times.Once);
    }

    [Fact]
    public async Task GetAllOfficesAsync_EmptyCollection_ReturnsEmptyList()
    {
        // Arrange
        var offices = new List<Office>();
        var dtos = new List<OfficeResponse>();

        _repoMock
            .Setup(r => r.GetAll())
            .Returns(offices.AsQueryable());

        _mapperMock
            .Setup(m => m.Map<List<OfficeResponse>>(offices))
            .Returns(dtos);

        // Act
        var result = await _service.GetAllOffices();

        // Assert
        result.Should().BeEmpty();
        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _mapperMock.Verify(m => m.Map<List<OfficeResponse>>(offices), Times.Once);
    }

    #endregion

    #region GetOfficeByIdAsync Tests

    [Fact]
    public async Task GetOfficeByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var office = TestHelpers.CreateTestOffice(name: "Main Office");
        var dto = new OfficeResponse
        {
            Id = office.Id,
            Name = office.Name,
            OfficeCode = 1000
        };

        _repoMock
            .Setup(r => r.GetByIdAsync(office.Id))
            .ReturnsAsync(office);

        _mapperMock
            .Setup(m => m.Map<OfficeResponse>(office))
            .Returns(dto);

        // Act
        var result = await _service.GetOfficeByIdAsync(office.Id);

        // Assert
        result.Should().BeEquivalentTo(dto);
        _repoMock.Verify(r => r.GetByIdAsync(office.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<OfficeResponse>(office), Times.Once);
    }

    [Fact]
    public async Task GetOfficeByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Office?)null);

        // Act
        var result = await _service.GetOfficeByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map<OfficeResponse>(It.IsAny<Office>()), Times.Never);
    }

    #endregion

    #region CreateOfficeAsync Tests

    [Theory]
    [InlineData("New York Office", 1001)]
    [InlineData("London Office", 1002)]
    [InlineData("Tokyo Office", 1003)]
    public async Task CreateOfficeAsync_ValidDto_ReturnsMappedDto(string officeName, int officeCode)
    {
        // Arrange
        var dto = new CreateOffice { Name = officeName };
        var office = TestHelpers.CreateTestOffice(name: officeName);
        var response = new OfficeResponse
        {
            Id = office.Id,
            Name = office.Name,
            OfficeCode = officeCode
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<Office>(dto))
            .Returns(office);

        _mapperMock
            .Setup(m => m.Map<OfficeResponse>(It.IsAny<Office>()))
            .Returns(response);

        // Act
        var result = await _service.CreateOfficeAsync(dto);

        // Assert
        result.Should().BeEquivalentTo(response);
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Office>(dto), Times.Once);
        _mapperMock.Verify(m => m.Map<OfficeResponse>(It.IsAny<Office>()), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Office>()), Times.Once);
    }

    [Fact]
    public async Task CreateOfficeAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateOffice { Name = "" };
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.CreateOfficeAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _createValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<Office>(It.IsAny<CreateOffice>()), Times.Never);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Office>()), Times.Never);
    }

    #endregion

    #region UpdateOfficeAsync Tests

    [Fact]
    public async Task UpdateOfficeAsync_WhenFound_UpdatesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateOffice
        {
            Id = id,
            Name = "Updated Office Name"
        };

        var existing = TestHelpers.CreateTestOffice(id: id, name: "Old Office Name");

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateOfficeAsync(dto);

        // Assert
        result.Should().BeTrue();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateOfficeAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateOffice { Id = id, Name = "Updated Office" };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Office?)null);

        // Act
        var result = await _service.UpdateOfficeAsync(dto);

        // Assert
        result.Should().BeFalse();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateOffice>(), It.IsAny<Office>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Office>()), Times.Never);
    }

    [Fact]
    public async Task UpdateOfficeAsync_InvalidDto_ThrowsValidationException()
    {
        // Arrange
        var dto = new UpdateOffice { Id = Guid.NewGuid(), Name = "" };
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name cannot be empty")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var act = async () => await _service.UpdateOfficeAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _updateValidatorMock.Verify(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateOffice>(), It.IsAny<Office>()), Times.Never);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Office>()), Times.Never);
    }

    #endregion

    #region DeleteOfficeAsync Tests

    [Fact]
    public async Task DeleteOfficeAsync_WhenFound_ReturnsTrue()
    {
        // Arrange
        var office = TestHelpers.CreateTestOffice();

        _repoMock
            .Setup(r => r.GetByIdAsync(office.Id))
            .ReturnsAsync(office);

        // Act
        var result = await _service.DeleteOfficeAsync(office.Id);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.GetByIdAsync(office.Id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(office.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteOfficeAsync_WhenNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Office?)null);

        // Act
        var result = await _service.DeleteOfficeAsync(id);

        // Assert
        result.Should().BeFalse();
        _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion
}