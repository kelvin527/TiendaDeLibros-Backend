using Microsoft.Extensions.Logging;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Helpers;
using TiendaDeLibros.Models;
using TiendaDeLibros.Repositories.Interfaces;
using TiendaDeLibros.Services.Interfaces;

namespace TiendaDeLibros.Services.Implementation;

public sealed class AuthorsService(IAuthorsRepository authorsRepository, ILogger<AuthorsService> logger) : IAuthorsService
{
    public async Task<PagedResponse<AuthorResponse>> GetAllAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var authors = await authorsRepository.GetAllAsync(cancellationToken);
        var responses = authors.Select(ToResponse).ToList();
        return PagingHelper.ToPagedResponse(responses, pageNumber, pageSize);
    }

    public async Task<AuthorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var author = await authorsRepository.GetByIdAsync(id, cancellationToken);
        if (author is null)
        {
            logger.LogWarning("Author {AuthorId} was not found", id);
            return null;
        }

        return ToResponse(author);
    }

    public async Task<IReadOnlyList<AuthorResponse>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var authors = await authorsRepository.GetByBookIdAsync(bookId, cancellationToken);
        return authors.Select(ToResponse).ToList();
    }

    public async Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            IdBook = request.IdBook,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var created = await authorsRepository.CreateAsync(author, cancellationToken);
        logger.LogInformation("Author {AuthorId} created", created.Id);
        return ToResponse(created);
    }

    public async Task<AuthorResponse?> UpdateAsync(
        int id, UpdateAuthorRequest request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            Id = id,
            IdBook = request.IdBook,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var updated = await authorsRepository.UpdateAsync(id, author, cancellationToken);
        if (updated is null)
        {
            logger.LogWarning("Attempted to update author {AuthorId}, but it was not found", id);
            return null;
        }

        logger.LogInformation("Author {AuthorId} updated", id);
        return ToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await authorsRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            logger.LogInformation("Author {AuthorId} deleted", id);
        }
        else
        {
            logger.LogWarning("Attempted to delete author {AuthorId}, but it was not found", id);
        }

        return deleted;
    }

    private static AuthorResponse ToResponse(Author author) =>
        new(author.Id, author.IdBook, author.FirstName, author.LastName);
}
