namespace EShop.Common.Security
{
    public interface IRijndaelEncryption
    {
        string Encryption(string plainText);
        string Decryption(string cipherText);
    }
}