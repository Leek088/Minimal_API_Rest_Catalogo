using MinimalApiRest.ApiEndpoints;
using MinimalApiRest.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

//Servi�os das extens�es
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

//Metodos de extens�es
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

//Usar os servi�os da classe ApplicationBuilderExtensions
var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware(environment)
    .UseAppCors();

app.UseHttpsRedirection();

// Ativar os servi�os de autenticar e autorizar
app.UseAuthentication();
app.UseAuthorization();

app.Run();
