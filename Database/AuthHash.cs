using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ShoppingCart.Database
{
    public class AuthHash
    {
        private byte[] Salt;

        public AuthHash()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            this.Salt = salt;
        }
        public string GetHash(string originValue)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: originValue,
                        salt: this.Salt,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));
        }
    }
}
