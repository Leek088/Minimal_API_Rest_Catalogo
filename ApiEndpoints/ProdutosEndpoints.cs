using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;
using MinimalApiRest.Models;

namespace MinimalApiRest.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        //Obter todos os produtos com filtro de 10 primeiros
        app.MapGet("/produtos", async (AppDbContext appDbContext) =>
        {
            var produtos = await appDbContext.Produtos.AsNoTracking().Take(10).ToListAsync();
            return Results.Ok(produtos);
        }).RequireAuthorization().WithTags("Produtos");

        //Obter um produto pelo Id
        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext appDbContext) =>
        {
            var produtos = await appDbContext.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produtos is not null)
                return Results.Ok(produtos);
            else
                return Results.NotFound("Produto não encontrado!");
        }).RequireAuthorization().WithTags("Produtos");

        //Adicionar um produto
        app.MapPost("/produtos", async (Produto produto, AppDbContext appDbContext) =>
        {
            appDbContext.Produtos.Add(produto);
            await appDbContext.SaveChangesAsync();

            return Results.Created($"/produtos/{produto.ProdutoId}", produto);
        }).RequireAuthorization().WithTags("Produtos");

        //Atualizar um produto
        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext appDbContext) =>
        {
            if (produto.ProdutoId != id)
                return Results.BadRequest();

            var produtoDB = await appDbContext.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produtoDB is null)
                return Results.NotFound();

            produtoDB.ProdutoId = produto.ProdutoId;
            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Preco = produto.Preco;
            produtoDB.Imagem = produto.Imagem;
            produtoDB.DataCompra = produto.DataCompra;
            produtoDB.Estoque = produto.Estoque;
            produtoDB.CategoriaId = produto.CategoriaId;

            await appDbContext.SaveChangesAsync();

            return Results.Ok(produtoDB);
        }).RequireAuthorization().WithTags("Produtos");

        //Deletar um produto
        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext appDbContext) =>
        {
            var produtos = await appDbContext.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produtos is null)
                return Results.NotFound();

            appDbContext.Produtos.Remove(produtos);
            await appDbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization().WithTags("Produtos");
    }
}
