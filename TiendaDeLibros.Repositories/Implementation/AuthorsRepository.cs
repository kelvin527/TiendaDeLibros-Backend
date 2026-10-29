using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TiendaDeLibros.Entities;
using TiendaDeLibros.Repositories.Interfaces;

namespace TiendaDeLibros.Repositories.Implementation;

public sealed class AuthorsRepository(HttpClient httpClient, ILogger<AuthorsRepository> logger) : IAuthorsRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all authors from the external API");
        var authors = await httpClient.GetFromJsonAsync<List<Author>>(ExternalApiRoutes.Authors.Base,
            JsonOptions, cancellationToken);
        return authors ?? [];
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(ExternalApiRoutes.Authors.ById(id), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Author {AuthorId} was not found in the external API", id);
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Author>(JsonOptions, cancellationToken);
    }

    public async Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(ExternalApiRoutes.Authors.ByBookId(bookId), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("No authors found for book {BookId}", bookId);
            return [];
        }

        response.EnsureSuccessStatusCode();
        var authors = await response.Content.ReadFromJsonAsync<List<Author>>(JsonOptions, cancellationToken);
        return authors ?? [];
    }

    public async Task<Author> CreateAsync(Author author, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(ExternalApiRoutes.Authors.Base, author, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Author>(JsonOptions, cancellationToken);
        logger.LogInformation("Created author {AuthorId}", created?.Id ?? author.Id);
        return created ?? author;
    }

    public async Task<Author?> UpdateAsync(int id, Author author, CancellationToken cancellationToken)
    {
        var response = await httpClient.PutAsJsonAsync(ExternalApiRoutes.Authors.ById(id), author, JsonOptions, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Attempted to update author {AuthorId}, but it was not found", id);
            return null;
        }

        response.EnsureSuccessStatusCode();
        logger.LogInformation("Updated author {AuthorId}", id);
        return await response.Content.ReadFromJsonAsync<Author>(JsonOptions, cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync(ExternalApiRoutes.Authors.ById(id), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("Attempted to delete author {AuthorId}, but it was not found", id);
            return false;
        }

        response.EnsureSuccessStatusCode();
        logger.LogInformation("Deleted author {AuthorId}", id);
        return true;
    }
}
