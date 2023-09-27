using System.Security.Cryptography;

namespace TodoApp.Service;


public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 1000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char SaltSeparator = ';';

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
        return string.Join(SaltSeparator, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }
    
    public static bool Verify(string password, string passwordHash)
    {
        string[] passwordElements = passwordHash.Split(SaltSeparator);
        if (passwordElements.Length != 2) return false;
        var salt = Convert.FromBase64String(passwordElements[0]);
        var hash = Convert.FromBase64String(passwordElements[1]);
        var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }

}