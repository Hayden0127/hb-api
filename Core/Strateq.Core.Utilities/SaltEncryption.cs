using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Strateq.Core.Utilities
{
    public class SaltEncryption
    {
        public static string hash(string plaintext, string salt)
        {
            var saltedPassword = plaintext + salt;
            SHA256 hashFunc = SHA256.Create();
            var plainBytes = Encoding.ASCII.GetBytes(saltedPassword);
            var toHash = new byte[saltedPassword.Length];
            plainBytes.CopyTo(toHash, 0);
            var hashPassword = hashFunc.ComputeHash(toHash);
            return Convert.ToBase64String(hashPassword, 0, hashPassword.Length);
        }

        public static string generateSalt()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var Bytesalt = new byte[100];
            rng.GetBytes(Bytesalt);
            return Convert.ToBase64String(Bytesalt, 0, Bytesalt.Length);
        }

        public static bool Equals(string modelHash, string userHash)
        {
            if (modelHash != userHash)
                return false;
            else
                return true;
        }
    }
}
