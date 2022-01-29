using Abstractions;

namespace Crypto
{
    public class EncryptionPackage : IEncryptionPackage
    {
        public string CipherText { get; set; }
        public string EncryptedKey { get; set; }
    }
}