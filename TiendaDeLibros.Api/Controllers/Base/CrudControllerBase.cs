using Microsoft.AspNetCore.Mvc;
using TiendaDeLibros.Models;
using TiendaDeLibros.Services;

namespace TiendaDeLibros.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TService, TResponse, TCreateRequest, TUpdateRequest>(TService service)
    : ControllerBase
    where TService : ICrudService<TResponse, TCreateRequest, TUpdateRequest>
    where TResponse : IIdentifiable
{
    protected TService Service { get; } = service;

    /// <summary>Obtiene una página de resultados.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<PagedResponse<TResponse>>> GetAll(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await Service.GetAllAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>Obtiene un registro por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await Service.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>Crea un nuevo registro.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public virtual async Task<ActionResult<TResponse>> Create(
        TCreateRequest request, CancellationToken cancellationToken)
    {
        var created = await Service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Actualiza un registro existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TResponse>> Update(
        int id, TUpdateRequest request, CancellationToken cancellationToken)
    {
        var updated = await Service.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Elimina un registro por su id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await Service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
