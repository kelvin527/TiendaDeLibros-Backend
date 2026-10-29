using Microsoft.AspNetCore.Mvc;
using TiendaDeLibros.Api.Controllers.Base;
using TiendaDeLibros.Models;
using TiendaDeLibros.Services.Interfaces;

namespace TiendaDeLibros.Api.Controllers;

public sealed class AuthorsController(IAuthorsService service)
    : CrudControllerBase<IAuthorsService, AuthorResponse, CreateAuthorRequest, UpdateAuthorRequest>(service)
{
    /// <summary>Obtiene los autores de un libro específico.</summary>
    [HttpGet("by-book/{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AuthorResponse>>> GetByBookId(
        int bookId, CancellationToken cancellationToken)
    {
        var authors = await Service.GetByBookIdAsync(bookId, cancellationToken);
        return Ok(authors);
    }
}
