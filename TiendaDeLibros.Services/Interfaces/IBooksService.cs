using TiendaDeLibros.Models;

namespace TiendaDeLibros.Services.Interfaces;

public interface IBooksService : ICrudService<BookResponse, CreateBookRequest, UpdateBookRequest>;
