using Microsoft.IdentityModel.Tokens;
using MinimalApiRest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApiRest.Services
{
    public class TokenService : ITokenService
    {
        public string GerarToken(string key, string issuer, string audience, UserModel user)
        {
            //Payload do token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };

            //Gerar Chave codificada, usando a chave secreta
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Aplicar o algoritimo na chave ,para recuperar uma chave simetrica
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            //Defenir a geração do token com emissor, audiencia, claims, duração do token e a chave simetrica
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            // Deserializar o token e retornar em string para o usuario
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;            
        }
    }
}
