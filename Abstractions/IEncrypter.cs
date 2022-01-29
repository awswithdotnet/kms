using System.Threading.Tasks;

namespace Abstractions
{
    public interface IEncrypter
    {
        Task<IEncryptionPackage> Encrypt(string plainText);
    }
}