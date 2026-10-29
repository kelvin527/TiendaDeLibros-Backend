# Tienda de Libros - API

API REST construida en .NET 8 que actúa como proxy entre el frontend y la API pública [FakeRestAPI](https://fakerestapi.azurewebsites.net/index.html), exponiendo endpoints propios para la gestión de libros y autores.

## Arquitectura

El proyecto está organizado por capas, siguiendo el principio de separación de responsabilidades:

```
TiendaDeLibros.Api              -> Controllers, middlewares y configuración de arranque
TiendaDeLibros.Services         -> Lógica de negocio y mapeo entre entidades y DTOs
TiendaDeLibros.Repositories     -> Acceso a la API externa (HttpClient), sin base de datos
TiendaDeLibros.Entities         -> Modelos de dominio internos
TiendaDeLibros.Models           -> DTOs de entrada/salida de la API (requests y responses)
TiendaDeLibros.Helpers          -> Utilidades compartidas (paginación)
TiendaDeLibros.Tests            -> Pruebas unitarias (xUnit + Moq)
```

No se utiliza base de datos: toda la información se obtiene en tiempo real desde la API externa.

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 o Visual Studio Code (opcional, también se puede usar la CLI de `dotnet`)

## Instalación y ejecución

1. Clonar el repositorio y entrar a la carpeta del proyecto.
2. Restaurar dependencias y compilar:

   ```bash
   dotnet restore
   dotnet build
   ```

3. Ejecutar la API:

   ```bash
   dotnet run --project TiendaDeLibros.Api
   ```

4. La API quedará disponible en `https://localhost:7xxx` (el puerto exacto se muestra en consola al arrancar). En ambiente de desarrollo, la documentación interactiva de Swagger está disponible en `/swagger`.

## Ejecutar las pruebas unitarias

```bash
dotnet test TiendaDeLibros.Tests
```

## Endpoints principales

### Libros

| Método | Ruta              | Descripción                  |
|--------|-------------------|-------------------------------|
| GET    | /api/books        | Lista paginada de libros (10 por página por defecto, `?pageNumber=&pageSize=`) |
| GET    | /api/books/{id}   | Obtiene un libro por id       |
| POST   | /api/books        | Crea un nuevo libro           |
| PUT    | /api/books/{id}   | Actualiza un libro existente  |
| DELETE | /api/books/{id}   | Elimina un libro              |

### Autores

| Método | Ruta                          | Descripción                        |
|--------|-------------------------------|-------------------------------------|
| GET    | /api/authors                  | Lista paginada de autores           |
| GET    | /api/authors/{id}             | Obtiene un autor por id             |
| GET    | /api/authors/by-book/{bookId} | Obtiene los autores de un libro     |
| POST   | /api/authors                  | Crea un nuevo autor                 |
| PUT    | /api/authors/{id}             | Actualiza un autor existente        |
| DELETE | /api/authors/{id}             | Elimina un autor                    |

## Configuración

En `TiendaDeLibros.Api/appsettings.json`:

- `BooksApi:BaseUrl`: URL base de la API externa consumida (FakeRestAPI).
- `Cors:AllowedOrigins`: orígenes permitidos para que el frontend pueda consumir esta API desde el navegador. Por defecto incluye los puertos típicos de desarrollo de React (`http://localhost:3000` para Create React App y `http://localhost:5173` para Vite). Si el frontend corre en otro puerto, agregarlo a esta lista.
