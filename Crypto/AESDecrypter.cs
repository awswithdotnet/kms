using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Abstractions;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace Crypto
{
    public class AESDecrypter : IDecrypter
    {
        private readonly string _keyId = "";
        private readonly byte[] _iv = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public async Task<string> Decrypt(IEncryptionPackage encryptionPackage)
        {

            AmazonKeyManagementServiceClient kmsClient = new AmazonKeyManagementServiceClient();

            MemoryStream ciphertextBlob = new MemoryStream(Convert.FromBase64String((encryptionPackage.EncryptedKey)));

            DecryptRequest decryptRequest = new DecryptRequest()
            {
                CiphertextBlob = ciphertextBlob,
                KeyId = _keyId
            };

            DecryptResponse decryptResponse = await kmsClient.DecryptAsync(decryptRequest);

            byte[] key = decryptResponse.Plaintext.ToArray();

            string plainText = String.Empty;

            using (Aes aes = Aes.Create())
            {

                byte[] byteData = Convert.FromBase64String(encryptionPackage.CipherText);


                using (MemoryStream memoryStream = new MemoryStream(byteData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(key, _iv), CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }

            }

            return plainText;

        }

    }
}
