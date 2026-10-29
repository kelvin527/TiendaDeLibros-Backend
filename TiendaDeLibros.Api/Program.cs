using System.Text.Json.Serialization;
using TiendaDeLibros.Api.Middleware;
using TiendaDeLibros.Repositories.Implementation;
using TiendaDeLibros.Repositories.Interfaces;
using TiendaDeLibros.Services.Implementation;
using TiendaDeLibros.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

const string FrontendCorsPolicy = "FrontendCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IBooksRepository, BooksRepository>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BooksApi:BaseUrl"]!);
});
builder.Services.AddScoped<IBooksService, BooksService>();

builder.Services.AddHttpClient<IAuthorsRepository, AuthorsRepository>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BooksApi:BaseUrl"]!);
});
builder.Services.AddScoped<IAuthorsService, AuthorsService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
