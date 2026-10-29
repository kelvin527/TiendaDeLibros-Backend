namespace TiendaDeLibros.Entities;

public sealed class Author
{
    public int Id { get; set; }

    public int IdBook { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}
