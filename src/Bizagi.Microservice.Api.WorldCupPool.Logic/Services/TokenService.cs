using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;
using Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.Options;
using Bizagi.Microservice.Api.WorldCupPool.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de generación de tokens JWT.
/// Utiliza la configuración definida en appsettings.json bajo la sección "Jwt".
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de tokens.
    /// </summary>
    /// <param name="jwtOptions">Opciones tipadas de configuración JWT.</param>
    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    /// <inheritdoc />
    public string GenerateToken(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    /// <inheritdoc />
    public int GetExpirationSeconds()
    {
        return _jwtOptions.ExpirationMinutes * 60;
    }
}
