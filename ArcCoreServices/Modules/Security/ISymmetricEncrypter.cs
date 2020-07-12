using System.Threading.Tasks;

namespace ArcCoreServices.Modules.Security
{
    public interface ISymmetricEncrypter
    {
        Task<string> EncryptString(string textToEncrypt);
        Task<byte[]> EncryptBytes(byte[] bytesToEncrypt);
        Task<string> DecryptString(string textToDecrypt);
        Task<byte[]> BytesToDecrypt(byte[] BytesToDecrypt);
    }
}