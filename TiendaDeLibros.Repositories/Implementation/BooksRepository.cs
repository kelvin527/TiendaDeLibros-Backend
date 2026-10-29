using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Repositories.Interfaces;

namespace TiendaDeLibros.Repositories.Implementation;

public sealed class BooksRepository(HttpClient httpClient, ILogger<BooksRepository> logger) : IBooksRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all books from the external API");
        var books = await httpClient.GetFromJsonAsync<List<Book>>(ExternalApiRoutes.Books.Base, JsonOptions, cancellationToken);
        return books ?? [];
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(ExternalApiRoutes.Books.ById(id), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Book {BookId} was not found in the external API", id);
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Book>(JsonOptions, cancellationToken);
    }

    public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(ExternalApiRoutes.Books.Base, book, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Book>(JsonOptions, cancellationToken);
        logger.LogInformation("Created book {BookId}", created?.Id ?? book.Id);
        return created ?? book;
    }

    public async Task<Book?> UpdateAsync(int id, Book book, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync(ExternalApiRoutes.Books.ById(id), book, JsonOptions, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Attempted to update book {BookId}, but it was not found", id);
            return null;
        }

        response.EnsureSuccessStatusCode();
        logger.LogInformation("Updated book {BookId}", id);
        return await response.Content.ReadFromJsonAsync<Book>(JsonOptions, cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync(ExternalApiRoutes.Books.ById(id), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Attempted to delete book {BookId}, but it was not found", id);
            return false;
        }

        response.EnsureSuccessStatusCode();
        logger.LogInformation("Deleted book {BookId}", id);
        return true;
    }
}
