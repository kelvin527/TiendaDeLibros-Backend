using TiendaDeLibros.Models;

namespace TiendaDeLibros.Services;

public interface ICrudService<TResponse, in TCreateRequest, in TUpdateRequest>
    where TResponse : IIdentifiable
{
    Task<PagedResponse<TResponse>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<TResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<TResponse> CreateAsync(TCreateRequest request, CancellationToken cancellationToken);

    Task<TResponse?> UpdateAsync(int id, TUpdateRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
