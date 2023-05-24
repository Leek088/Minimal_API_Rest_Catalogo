using Microsoft.EntityFrameworkCore;
using MinimalApiRest.Context;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
