using TiendaDeLibros.Entities;

namespace TiendaDeLibros.Repositories.Interfaces;

public interface IAuthorsRepository
{
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken);

    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Author>> GetByBookIdAsync(int bookId, CancellationToken cancellationToken);

    Task<Author> CreateAsync(Author author, CancellationToken cancellationToken);

    Task<Author?> UpdateAsync(int id, Author author, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
