using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;
using MinimalApiRest.Models;

namespace MinimalApiRest.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        //Obter todas as categorias com filtro de 10 primeiras
        app.MapGet("/categorias", async (AppDbContext appDbContext) =>
        {
            var categorias = await appDbContext.Categorias.AsNoTracking().Take(10).ToListAsync();
            return Results.Ok(categorias);
        }).RequireAuthorization().WithTags("Categorias");

        //Obter a categoria pelo Id
        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext appDbContext) =>
        {
            var categorias = await appDbContext.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categorias is not null)
                return Results.Ok(categorias);
            else
                return Results.NotFound("Categoria não encontrada!");
        }).RequireAuthorization().WithTags("Categorias"); ;

        //Adicionar uma cetegoria
        app.MapPost("/categorias", async (Categoria categoria, AppDbContext appDbContext) =>
        {
            appDbContext.Categorias.Add(categoria);
            await appDbContext.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        }).RequireAuthorization().WithTags("Categorias");

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
        }).RequireAuthorization().WithTags("Categorias");

        //Deletar uma categoria
        app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext appDbContext) =>
        {
            var categoria = await appDbContext.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoria is null)
                return Results.NotFound();

            appDbContext.Categorias.Remove(categoria);
            await appDbContext.SaveChangesAsync();

            return Results.NoContent();

        }).RequireAuthorization().WithTags("Categorias");
    }
}
