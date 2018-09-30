using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Utilize.Identity.Shared.Services
{
    public interface IPasswordService
    {
        string CreateSalt();
        string GetHash(string password, string saltAsBase64String);
        bool ValidatePassword(string password, string salt, string hashAsBase64String);
    }

    public class PkcsSha256PasswordService : IPasswordService
    {
        private readonly SecureRandom _cryptoRandom;
        private const int Iterations = 100000;
        private const int SaltByteSize = 64;
        private const int HashByteSize = 128;

        public PkcsSha256PasswordService()
        {
            _cryptoRandom = new SecureRandom();
        }

        public string CreateSalt()
        {
            var salt = new byte[SaltByteSize];
            _cryptoRandom.NextBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public string GetHash(string password, string saltAsBase64String)
        {
            var saltBytes = Convert.FromBase64String(saltAsBase64String);

            var hash = GetHash(password, saltBytes);

            return Convert.ToBase64String(hash);
        }

        private static byte[] GetHash(string password, byte[] salt)
        {
            var pdb = new Pkcs5S2ParametersGenerator(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());
            pdb.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt,
                Iterations);
            var key = (KeyParameter) pdb.GenerateDerivedMacParameters(HashByteSize * 8);
            return key.GetKey();
        }

        public bool ValidatePassword(string password, string salt, string hashAsBase64String)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var actualHashBytes = Convert.FromBase64String(hashAsBase64String);
            return ValidatePassword(password, saltBytes, actualHashBytes);
        }

        private static bool ValidatePassword(string password, byte[] saltBytes, IReadOnlyList<byte> actualGainedHasAsByteArray)
        {
            var testHash = GetHash(password, saltBytes);
            return SlowEquals(actualGainedHasAsByteArray, testHash);
        }

        private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var diff = (uint) a.Count ^ (uint) b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
                diff |= (uint) (a[i] ^ b[i]);
            return diff == 0;
        }
    }
}