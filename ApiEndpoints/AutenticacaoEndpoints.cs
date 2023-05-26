using Microsoft.AspNetCore.Authorization;
using MinimalApiRest.Models;
using MinimalApiRest.Services;

namespace MinimalApiRest.ApiEndpoints;

public static class AutenticacaoEndpoints
{
    public static void MapAutenticacaoEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel is null)
                return Results.BadRequest("Login Inválido!");

            if (userModel.UserName == "usuario1" && userModel.Password == "senha")
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"]!,
                                  app.Configuration["Jwt:Issuer"]!,
                                  app.Configuration["Jwt:Audience"]!,
                                  userModel);

                return Results.Ok(new { token = tokenString });
            }
            else
                return Results.BadRequest("Login Inválido!");
        }).RequireAuthorization().WithTags("Autorização");
    }
}
