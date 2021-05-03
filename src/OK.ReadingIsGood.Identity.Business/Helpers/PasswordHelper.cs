using System;
using System.Linq;
using System.Security.Cryptography;

namespace OK.ReadingIsGood.Identity.Business.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            using var algorithm = new Rfc2898DeriveBytes(
              password,
              SaltSize,
              Iterations,
              HashAlgorithmName.SHA512);

            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Iterations}.{salt}.{key}";
        }

        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentException(nameof(hash));
            }

            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new ArgumentException(nameof(hash));
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(
              password,
              salt,
              iterations,
              HashAlgorithmName.SHA512);

            var keyToCheck = algorithm.GetBytes(KeySize);

            return keyToCheck.SequenceEqual(key);
        }
    }
}