using TiendaDeLibros.Entities;

namespace TiendaDeLibros.Repositories.Interfaces;

public interface IBooksRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Book> CreateAsync(Book book, CancellationToken cancellationToken);

    Task<Book?> UpdateAsync(int id, Book book, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
