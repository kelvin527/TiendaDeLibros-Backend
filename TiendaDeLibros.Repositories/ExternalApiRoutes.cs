namespace TiendaDeLibros.Repositories;

internal static class ExternalApiRoutes
{
    internal static class Books
    {
        internal const string Base = "api/v1/Books";
        internal static string ById(int id) => $"{Base}/{id}";
    }

    internal static class Authors
    {
        internal const string Base = "api/v1/Authors";
        internal static string ById(int id) => $"{Base}/{id}";
        internal static string ByBookId(int bookId) => $"{Base}/authors/books/{bookId}";
    }
}
