using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Models;
using TiendaDeLibros.Repositories.Interfaces;
using TiendaDeLibros.Services.Implementation;
using Xunit;

namespace TiendaDeLibros.Tests;

public class BooksServiceTests
{
    private readonly Mock<IBooksRepository> _repository = new();
    private readonly BooksService _sut;

    public BooksServiceTests()
    {
        _sut = new BooksService(_repository.Object, NullLogger<BooksService>.Instance);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedResponse_MappedFromRepository()
    {
        var books = new List<Book>
        {
            new() { Id = 1, Title = "Book 1", PublishDate = DateTimeOffset.UtcNow },
            new() { Id = 2, Title = "Book 2", PublishDate = DateTimeOffset.UtcNow }
        };
        _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(books);

        var result = await _sut.GetAllAsync(pageNumber: 1, pageSize: 10, CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Collection(result.Items,
            b => Assert.Equal("Book 1", b.Title),
            b => Assert.Equal("Book 2", b.Title));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenRepositoryReturnsNull()
    {
        _repository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var result = await _sut.GetByIdAsync(99, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedResponse_WhenFound()
    {
        var book = new Book { Id = 5, Title = "Found Book", PublishDate = DateTimeOffset.UtcNow };
        _repository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(book);

        var result = await _sut.GetByIdAsync(5, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result!.Id);
        Assert.Equal("Found Book", result.Title);
    }

    [Fact]
    public async Task CreateAsync_PassesMappedEntity_ToRepository()
    {
        var request = new CreateBookRequest
        {
            Title = "New Book",
            PageCount = 100,
            PublishDate = DateTimeOffset.UtcNow
        };
        _repository.Setup(r => r.CreateAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book b, CancellationToken _) => b);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        Assert.Equal("New Book", result.Title);
        _repository.Verify(r => r.CreateAsync(
            It.Is<Book>(b => b.Title == "New Book" && b.PageCount == 100),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenRepositoryReturnsFalse()
    {
        _repository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        Assert.False(result);
    }
}
