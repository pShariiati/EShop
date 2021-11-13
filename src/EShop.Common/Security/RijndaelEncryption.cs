using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Common.Security;

public class RijndaelEncryption : IRijndaelEncryption
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public RijndaelEncryption(IConfiguration configuration)
    {
        _key = Encoding.UTF8.GetBytes(configuration["Rijndael:Key"]);
        _iv = Encoding.UTF8.GetBytes(configuration["Rijndael:IV"]);
    }
    public string Encryption(string plainText)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException(nameof(plainText));
        // Create an RijndaelManaged object
        // with the specified key and IV.
        using var rijAlg = new RijndaelManaged { Key = _key, IV = _iv };

        // Create an encryptor to perform the stream transform.
        var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

        // Create the streams used for encryption.
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            //Write all data to the stream.
            swEncrypt.Write(plainText);
        }
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public string Decryption(string cipherText)
    {
        var cipherTextBytes = Convert.FromBase64String(cipherText);
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException(nameof(cipherText));

        // Create an RijndaelManaged object
        // with the specified key and IV.
        using var rijAlg = new RijndaelManaged { Key = _key, IV = _iv };

        // Create a decryptor to perform the stream transform.
        var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        // Create the streams used for decryption.
        using var msDecrypt = new MemoryStream(cipherTextBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        // Read the decrypted bytes from the decrypting stream
        // and place them in a string.
        return srDecrypt.ReadToEnd();
    }
}
