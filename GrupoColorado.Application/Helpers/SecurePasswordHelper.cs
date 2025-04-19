using System.Security.Cryptography;

namespace GrupoColorado.Application.Helpers
{
  public static class SecurePasswordHelper
  {
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
      using RandomNumberGenerator rng = RandomNumberGenerator.Create();
      byte[] salt = new byte[SaltSize];
      rng.GetBytes(salt);

      using Rfc2898DeriveBytes pbkdf2 = new(password, salt, Iterations, HashAlgorithmName.SHA256);
      byte[] key = pbkdf2.GetBytes(KeySize);

      var hashParts = new
      {
        Iterations,
        Salt = Convert.ToBase64String(salt),
        Key = Convert.ToBase64String(key)
      };

      return $"{hashParts.Iterations}.{hashParts.Salt}.{hashParts.Key}";
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
      string[] parts = storedHash.Split('.');
      if (parts.Length != 3)
        return false;

      int iterations = int.Parse(parts[0]);
      byte[] salt = Convert.FromBase64String(parts[1]);
      byte[] key = Convert.FromBase64String(parts[2]);

      using Rfc2898DeriveBytes pbkdf2 = new(password, salt, iterations, HashAlgorithmName.SHA256);
      byte[] keyToCheck = pbkdf2.GetBytes(KeySize);

      return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
  }
}