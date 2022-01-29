using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Abstractions;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace Crypto
{
    public class AESEncrypter : IEncrypter
    {
        private readonly string _keyId = "";
        private readonly byte[] _iv = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public async Task<IEncryptionPackage> Encrypt(string plainText)
        {
            byte[] encryptedData;
            AmazonKeyManagementServiceClient kmsClient = new AmazonKeyManagementServiceClient();

            GenerateDataKeyRequest dataKeyRequest = new GenerateDataKeyRequest()
            {
                KeyId = _keyId,
                KeySpec = DataKeySpec.AES_256
            };

            GenerateDataKeyResponse dataKeyResponse = await kmsClient.GenerateDataKeyAsync(dataKeyRequest);

            byte[] encryptedDataKey = dataKeyResponse.CiphertextBlob.ToArray();
            byte[] plainTextKey = dataKeyResponse.Plaintext.ToArray();

            using (Aes aes = Aes.Create())
            {

                ICryptoTransform cryptoTransform = aes.CreateEncryptor(plainTextKey, _iv);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            await streamWriter.WriteAsync(plainText);
                        }

                        encryptedData = memoryStream.ToArray();
                    }
                }
            }

            string encryptedString = Convert.ToBase64String(encryptedData);

            IEncryptionPackage encryptionPackage = new EncryptionPackage
            {
                CipherText = encryptedString,
                EncryptedKey = Convert.ToBase64String(encryptedDataKey)
            };

            return encryptionPackage;

        }

    }
}
