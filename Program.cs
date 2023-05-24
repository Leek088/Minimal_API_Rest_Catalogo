using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;
using MinimalApiRest.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Recuperar a string de conexão
var connectionStringMyql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Incluir o serviço de contexto.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionStringMyql, ServerVersion.AutoDetect(connectionStringMyql)));

var app = builder.Build();

// ------------------------------------ EndPoints Categorias -------------------------------------

//Obter todas as categorias com filtro de 10 primeiras
app.MapGet("/categorias", async (AppDbContext appDbContext) =>
{
    var categorias = await appDbContext.Categorias.AsNoTracking().Take(10).ToListAsync();
    return Results.Ok(categorias);
});

//Obter a categoria pelo Id
app.MapGet("/categorias/{id:int}", async (int id, AppDbContext appDbContext) =>
{
    var categorias = await appDbContext.Categorias
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.CategoriaId == id);

    if (categorias is Categoria)
        return Results.Ok(categorias);
    else
        return Results.NotFound("Produto não encontrado!");
});

//Adicionar uma cetegoria
app.MapPost("/categorias", async (Categoria categoria, AppDbContext appDbContext) =>
{
    appDbContext.Categorias.Add(categoria);
    await appDbContext.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

//Atualizar uma categoria
app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext appDbContext) =>
{
    if (categoria.CategoriaId != id)
        return Results.BadRequest();

    var categoriaDB = await appDbContext.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

    if (categoriaDB is null)
        return Results.NotFound();
    
    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await appDbContext.SaveChangesAsync();

    return Results.Ok(categoriaDB);
});

//Deletar uma categoria
app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext appDbContext) =>
{
    var categoria = await appDbContext.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

    if (categoria is null)
        return Results.NotFound();

    appDbContext.Categorias.Remove(categoria);
    await appDbContext.SaveChangesAsync();

    return Results.NoContent();

});

// ------------------------------------ EndPoints Produtos -------------------------------------

//Obter todos os produtos com filtro de 10 primeiros
app.MapGet("/produtos", async (AppDbContext appDbContext) =>
{
    var produtos = await appDbContext.Produtos.AsNoTracking().Take(10).ToListAsync();
    return Results.Ok(produtos);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
