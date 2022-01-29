namespace Abstractions
{
    public interface IEncryptionPackage
    {
        string CipherText { get; set; }
        string EncryptedKey { get; set; }
    }
}