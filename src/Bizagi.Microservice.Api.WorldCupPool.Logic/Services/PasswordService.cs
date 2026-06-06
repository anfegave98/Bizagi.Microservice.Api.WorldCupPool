using System.Security.Cryptography;
using System.Text;
using Bizagi.Microservice.Api.WorldCupPool.Abstractions.Services;

namespace Bizagi.Microservice.Api.WorldCupPool.Logic.Services;

/// <summary>
/// Implementación del servicio de hash y verificación de contraseñas usando HMACSHA512.
/// Las contraseñas nunca se almacenan ni retornan en texto plano.
/// </summary>
public class PasswordService : IPasswordService
{
    /// <inheritdoc />
    public void CreateHash(string password, out string passwordHash, out string passwordSalt)
    {
        using var hmac = new HMACSHA512();
        var saltBytes = hmac.Key;
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        passwordSalt = Convert.ToBase64String(saltBytes);
        passwordHash = Convert.ToBase64String(hashBytes);
    }

    /// <inheritdoc />
    public bool VerifyHash(string password, string passwordHash, string passwordSalt)
    {
        var saltBytes = Convert.FromBase64String(passwordSalt);
        using var hmac = new HMACSHA512(saltBytes);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var storedHash = Convert.FromBase64String(passwordHash);
        return computedHash.SequenceEqual(storedHash);
    }
}
