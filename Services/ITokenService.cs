using MinimalApiRest.Models;

namespace MinimalApiRest.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, UserModel user);
}
