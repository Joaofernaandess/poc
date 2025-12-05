using System.Runtime.CompilerServices;
using GestãoApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ClienteRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapPost("/Cliente", async (Cliente cliente, ClienteRepository repo) =>
{
    if(string.IsNullOrEmpty(cliente.Nome))
    {
        return Results.BadRequest("Ei! O nome é obrigatório.");
    }
    await repo.CreateAsync(cliente);
    return Results.Created($"/Cliente/{cliente.Clienteid}", cliente);
});

app.MapGet("/Cliente", async(ClienteRepository repo) =>
{
    var lista = await repo.GetAllAsync();
    return Results.Ok(lista);
});

app.MapGet("/clientes/{id}", async (Guid id, ClienteRepository repo) =>
{
    var cliente = await repo.GetByIdAsync(id);
    if (cliente == null) return Results.NotFound("Cliente não existe!");

    return Results.Ok(cliente);
});

app.MapDelete("/cliente/{id}", async (Guid id, ClienteRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.Ok("Cliente removido com sucesso!");
});
app.Run();
