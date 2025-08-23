namespace AdoptaYA.Services.Security;
public class EncryptionService
{
    public string Encrypt (string password)
    {
        return UniEncryption.AesEncrypt.Encrypt(password!)?.ToString()!;
    }

    public string Decrypt (string password)
    {
        return UniEncryption.AesEncrypt.Decrypt(password!)?.ToString()!;
    }
}