<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;
using MinimalApiRest.Models;
using MinimalApiRest.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
=======
using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;
>>>>>>> Adicionar arquivos de projeto.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
//Recuperar a string de conexão
var connectionStringMyql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Incluir o serviço de contexto.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionStringMyql, ServerVersion.AutoDetect(connectionStringMyql)));

// Registro do serviço do Token
builder.Services.AddSingleton<ITokenService>(new TokenService());

// Registro do serviço de validação do Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});

// Incluir o serviço de autorização
builder.Services.AddAuthorization();


var app = builder.Build();

// Endpoint Login
app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if (userModel is null)
        return Results.BadRequest("Login Inválido!");

    if (userModel.UserName == "usuario" && userModel.Password == "senhaUser")
    {
        var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"]!,
                          app.Configuration["Jwt:Issuer"]!,
                          app.Configuration["Jwt:Audience"]!,
                          userModel);

        return Results.Ok(new { token = tokenString });
    }
    else
        return Results.BadRequest("Login Inválido!");

});


// ------------------------------------ EndPoints Categorias -------------------------------------

//Obter todas as categorias com filtro de 10 primeiras
app.MapGet("/categorias", async (AppDbContext appDbContext) =>
{
    var categorias = await appDbContext.Categorias.AsNoTracking().Take(10).ToListAsync();
    return Results.Ok(categorias);
}).RequireAuthorization();

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
}).RequireAuthorization();

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
});

//Adicionar um produto
app.MapPost("/produtos", async (Produto produto, AppDbContext appDbContext) =>
{
    appDbContext.Produtos.Add(produto);
    await appDbContext.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
});

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
});

//Deletar um produto
app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext appDbContext) =>
{
    var produtos = await appDbContext.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

    if (produtos is null)
        return Results.NotFound();

    appDbContext.Produtos.Remove(produtos);
    await appDbContext.SaveChangesAsync();

    return Results.NoContent();
});


=======
//Obter a string de conexão
var connectionStringMysql = builder.Configuration.GetConnectionString("connectionMysql");

//Incluir o serviço no contexto
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionStringMysql, ServerVersion.AutoDetect(connectionStringMysql)));

var app = builder.Build();

>>>>>>> Adicionar arquivos de projeto.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

<<<<<<< HEAD
// Ativar os serviços de autenticar e autorizar
app.UseAuthentication();
app.UseAuthorization();
=======
>>>>>>> Adicionar arquivos de projeto.

app.Run();
