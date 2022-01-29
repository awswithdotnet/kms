using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Abstractions;
using Crypto;

namespace Sender
{
    class Program
    {
        private const string RequestUri = "http://localhost:5000/Message";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter message:");
            
            var message = Console.ReadLine();
            
            Console.WriteLine("Original: " + message);

            IEncrypter encrypter = new AESEncrypter();

            IEncryptionPackage encryptionPackage = await encrypter.Encrypt(message);

            string jsonString = JsonSerializer.Serialize(encryptionPackage);

            using (HttpClient httpClient = new HttpClient())
            {
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("text/plain")
                );

                HttpResponseMessage response = await httpClient.PostAsync(RequestUri, httpContent);

                string decryptedString = await response.Content.ReadAsStringAsync();

                Console.Write("Decrypted: " + decryptedString);

            }
        }
    }
}