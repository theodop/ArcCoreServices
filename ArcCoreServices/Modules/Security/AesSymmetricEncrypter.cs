using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArcCoreServices.Modules.Security
{
    public class AesSymmetricEncrypter : ISymmetricEncrypter
    {

        private UTF8Encoding encoding = new UTF8Encoding();
        private Func<byte[]> _keyGetFunc;

        const int keySize = 256;
        private byte[] _aesKey;
        private byte[] GetAesKey()
        {
            
            if (_aesKey == null)
            {
                _aesKey = _keyGetFunc();
                if (_aesKey == null) throw new ArgumentNullException(nameof(_aesKey), "AES key not defined");
                if (_aesKey.Length != keySize) throw new InvalidDataException($"Expected key size {keySize} but got {_aesKey.Length}");
            }

            return _aesKey;
        
        }

        public AesSymmetricEncrypter(Func<byte[]> keyGetFunc)
        {
            _keyGetFunc = keyGetFunc;
        }

        public Task<byte[]> BytesToDecrypt(byte[] BytesToDecrypt)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> DecryptString(string textToDecrypt)
        {
            throw new System.NotImplementedException();
        }

        public async Task<byte[]> EncryptBytes(byte[] bytesToEncrypt)
        {
            var aesKey = GetAesKey();

            RijndaelManaged rm = new RijndaelManaged
            {
                KeySize = keySize,
                Key = aesKey
            };
            rm.GenerateIV();

            var encryptor = rm.CreateEncryptor();

            byte[] encryptedBytes = await Task.Run(() => encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            byte[] totalBytes = new byte[encryptedBytes.Length + rm.IV.Length];

            int ivLength = rm.IV.Length;

            for (int i = 0; i < totalBytes.Length; i++)
            {
                if (i < ivLength)
                {
                    totalBytes[i] = rm.IV[i];
                }
                else
                {
                    totalBytes[i] = encryptedBytes[i - ivLength];
                }
            }

            return totalBytes;
        }

        public async Task<string> EncryptString(string textToEncrypt)
        {
            var encryptedBytes = await EncryptBytes(encoding.GetBytes(textToEncrypt));

            return encoding.GetString(encryptedBytes, 0, encryptedBytes.Length);
        }
    }
}