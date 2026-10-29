using TiendaDeLibros.Models;

namespace TiendaDeLibros.Services.Interfaces;

public interface IAuthorsService : ICrudService<AuthorResponse, CreateAuthorRequest, UpdateAuthorRequest>
{
    Task<IReadOnlyList<AuthorResponse>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken);
}
