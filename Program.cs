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
    await repo.CreatAsync(cliente);
    return Results.Created($"/clientes/{cliente.Clienteid}", cliente);
})
.WithOpenApi();

app.MapGet("/cliente", Async(ClienteRepository repo) =>
{
    var lista = await repo.GetAllAsync();
    return Results.Ok(lista);
})

.WithOpenApi();

app.MapGet("/clientes/{id}", async (Guid id, ClienteRepository repo) =>
{
    var cliente = await GetBIdAsnc(id);
    if (cliente == null) return Results.NotFound("Cliente não existe!");

    return Results.Ok(cliente);
})

.WithOpenApi();

app.MapDelete("/cliente/{id}", async (Guid id, ClienteRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.ok("Cliente removido com sucesso!");
})
.WithOpenApi();
app.run();
