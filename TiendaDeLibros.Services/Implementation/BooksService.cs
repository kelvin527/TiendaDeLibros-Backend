using Microsoft.Extensions.Logging;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Helpers;
using TiendaDeLibros.Models;
using TiendaDeLibros.Repositories.Interfaces;
using TiendaDeLibros.Services.Interfaces;

namespace TiendaDeLibros.Services.Implementation;

public sealed class BooksService(IBooksRepository booksRepository, ILogger<BooksService> logger) : IBooksService
{
    public async Task<PagedResponse<BookResponse>> GetAllAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var books = await booksRepository.GetAllAsync(cancellationToken);
        var responses = books.Select(ToResponse).ToList();
        return PagingHelper.ToPagedResponse(responses, pageNumber, pageSize);
    }

    public async Task<BookResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var book = await booksRepository.GetByIdAsync(id, cancellationToken);
        if (book is null)
        {
            logger.LogWarning("Book {BookId} was not found", id);
            return null;
        }

        return ToResponse(book);
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = request.Title,
            Description = request.Description,
            PageCount = request.PageCount,
            Excerpt = request.Excerpt,
            PublishDate = request.PublishDate
        };

        var created = await booksRepository.CreateAsync(book, cancellationToken);
        logger.LogInformation("Book {BookId} created", created.Id);
        return ToResponse(created);
    }

    public async Task<BookResponse?> UpdateAsync(
        int id, UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            PageCount = request.PageCount,
            Excerpt = request.Excerpt,
            PublishDate = request.PublishDate
        };

        var updated = await booksRepository.UpdateAsync(id, book, cancellationToken);
        if (updated is null)
        {
            logger.LogWarning("Attempted to update book {BookId}, but it was not found", id);
            return null;
        }

        logger.LogInformation("Book {BookId} updated", id);
        return ToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await booksRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            logger.LogInformation("Book {BookId} deleted", id);
        }
        else
        {
            logger.LogWarning("Attempted to delete book {BookId}, but it was not found", id);
        }

        return deleted;
    }

    private static BookResponse ToResponse(Book book) =>
        new(book.Id, book.Title, book.Description, book.PageCount, book.Excerpt, book.PublishDate);
}
