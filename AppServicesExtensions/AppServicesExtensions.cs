using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiRest.Context;
using MinimalApiRest.Services;
using System.Text;

namespace MinimalApiRest.AppServicesExtensions;

public static class AppServicesExtensions
{
    public static WebApplicationBuilder AddApiSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwagger();
        return builder;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddEndpointsApiExplorer();
        //Conigurar o Swagger para login e token JWT
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCatalogo", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer'[space].Example: \'Bearer 12345abcdef\'",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         Array.Empty<string>()
                    }
                });
        });

        return services;
    }

    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        //Recuperar a string de conexão
        var connectionStringMyql = builder.Configuration.GetConnectionString("ConnectionMysql");

        // Incluir o serviço de contexto.
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionStringMyql, ServerVersion.AutoDetect(connectionStringMyql)));

        // Registro do serviço do Token
        builder.Services.AddSingleton<ITokenService>(new TokenService());

        return builder;
    }

    public static WebApplicationBuilder AddAutenticationJwt(this WebApplicationBuilder builder)
    {
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

        return builder;
    }
}
