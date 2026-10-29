using TiendaDeLibros.Api.Controllers.Base;
using TiendaDeLibros.Models;
using TiendaDeLibros.Services.Interfaces;

namespace TiendaDeLibros.Api.Controllers;

public sealed class BooksController(IBooksService service)
    : CrudControllerBase<IBooksService, BookResponse, CreateBookRequest, UpdateBookRequest>(service);
