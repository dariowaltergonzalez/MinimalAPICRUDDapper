using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPICRUDDapper.Entidades;
using MinimalAPICRUDDapper.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddTransient<IRepositorioPersonas, RepositorioPersonas> ();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/personas",async (IRepositorioPersonas repositorioPersonas) =>
{
    var personas = await repositorioPersonas.ObtenerTodos();
    return Results.Ok(personas);
});

app.MapGet("/personas/{id:int}", async 
    Task<Results<NotFound, Ok<Persona>>>
    (IRepositorioPersonas repositorioPersonas, int id) =>
{
    var persona = await repositorioPersonas.ObtenerPorId(id);
        
    return persona is not null ? TypedResults.Ok(persona) : TypedResults.NotFound();
});

app.MapPost("/personas", async (IRepositorioPersonas repositorioPersonas, Persona persona) =>
{
    await repositorioPersonas.Crear(persona);
    return TypedResults.Created($"/personas/{persona.Id}", persona);
});


app.MapPut("/personas/{id:int}", async
    Task<Results<BadRequest<string>, NoContent, NotFound>>
    (int id, Persona persona, IRepositorioPersonas repositorioPersonas ) =>
{
    if (id != persona.Id)
    {
        return TypedResults.BadRequest("El id en la URL no coincide con el id en el cuerpo");
    }
    var existe = await repositorioPersonas.ExistePorId(id);

    if (!existe)
    {
        return TypedResults.NotFound();
    }

    await repositorioPersonas.Actualizar(persona);
    return TypedResults.NoContent();
});


app.MapDelete("/personas/{id:int}", async
    Task<Results<BadRequest<string>, NoContent, NotFound>>
    (int id, IRepositorioPersonas repositorioPersonas) =>
{
    var existe = await repositorioPersonas.ExistePorId(id);

    if (!existe)
    {
        return TypedResults.NotFound();
    }

    await repositorioPersonas.Borrar(id);
    return TypedResults.NoContent();
});


app.Run();


