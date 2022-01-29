using System.Threading.Tasks;

namespace Abstractions
{
public interface IDecrypter
{
    Task<string> Decrypt(IEncryptionPackage encryptionPackage);
}
}
