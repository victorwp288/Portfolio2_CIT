using System.Security.Cryptography;
using System.Text;

namespace DataAcessLayer.HashClass;

public class Hasher
{
    private const int SaltBitSize = 64; // Salt size in bits
    private const int Iterations = 600000; // Number of iterations for the hash
    private const int HashByteSize = 32; // Output hash size in bytes

    private readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();
    private readonly HashAlgorithm _hashAlgorithm = SHA256.Create();

    public Tuple<string, string> HashPassword(string password)
    {
        // Generate a unique salt
        byte[] salt = new byte[SaltBitSize / 8];
        _random.GetBytes(salt);
        string saltString = Convert.ToHexString(salt);

        // Generate the hash
        string hash = ComputeIterativeHash(password, saltString);

        return Tuple.Create(hash, saltString);
    }

    public bool VerifyPassword(string password, string storedHash, string salt)
    {
        string computedHash = ComputeIterativeHash(password, salt);

        if (storedHash == computedHash)
        {
            return true;
        }

        else
            return false;
    }

    private string ComputeIterativeHash(string password, string salt)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(salt + password);
        byte[] hashBytes = inputBytes;

        for (int i = 0; i < Iterations; i++)
        {
            hashBytes = _hashAlgorithm.ComputeHash(hashBytes);
        }

        return Convert.ToHexString(hashBytes);
    }
}
