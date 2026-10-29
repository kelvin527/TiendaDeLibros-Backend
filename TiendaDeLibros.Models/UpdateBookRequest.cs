using System.ComponentModel.DataAnnotations;

namespace TiendaDeLibros.Models;

/// <summary>Datos necesarios para actualizar un libro existente.</summary>
public sealed record UpdateBookRequest
{
    [Required, MaxLength(200)]
    public required string Title { get; init; }

    public string? Description { get; init; }

    [Range(0, int.MaxValue)]
    public int PageCount { get; init; }

    public string? Excerpt { get; init; }

    public required DateTimeOffset PublishDate { get; init; }
}
