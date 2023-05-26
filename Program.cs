using MinimalApiRest.ApiEndpoints;
using MinimalApiRest.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

//Serviços das extensões
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

//Metodos de extensões
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

//Usar os serviços da classe ApplicationBuilderExtensions
var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware(environment)
    .UseAppCors();

app.UseHttpsRedirection();

// Ativar os serviços de autenticar e autorizar
app.UseAuthentication();
app.UseAuthorization();

app.Run();
