using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Models;
using TiendaDeLibros.Repositories.Interfaces;
using TiendaDeLibros.Services.Implementation;
using Xunit;

namespace TiendaDeLibros.Tests;

public class AuthorsServiceTests
{
    private readonly Mock<IAuthorsRepository> _repository = new();
    private readonly AuthorsService _sut;

    public AuthorsServiceTests()
    {
        _sut = new AuthorsService(_repository.Object, NullLogger<AuthorsService>.Instance);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedResponse_MappedFromRepository()
    {
        var authors = new List<Author>
        {
            new() { Id = 1, IdBook = 1, FirstName = "Ana", LastName = "Garcia" }
        };
        _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(authors);

        var result = await _sut.GetAllAsync(pageNumber: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Ana", result.Items[0].FirstName);
    }

    [Fact]
    public async Task GetByBookIdAsync_ReturnsMappedAuthors()
    {
        var authors = new List<Author>
        {
            new() { Id = 1, IdBook = 7, FirstName = "Juan", LastName = "Perez" },
            new() { Id = 2, IdBook = 7, FirstName = "Maria", LastName = "Lopez" }
        };
        _repository.Setup(r => r.GetByBookIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(authors);

        var result = await _sut.GetByBookIdAsync(7, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal(7, a.IdBook));
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRepositoryReturnsNull()
    {
        var request = new UpdateAuthorRequest { IdBook = 1, FirstName = "X", LastName = "Y" };
        _repository.Setup(r => r.UpdateAsync(42, It.IsAny<Author>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Author?)null);

        var result = await _sut.UpdateAsync(42, request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenRepositorySucceeds()
    {
        _repository.Setup(r => r.DeleteAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.DeleteAsync(3, CancellationToken.None);

        Assert.True(result);
    }
}
