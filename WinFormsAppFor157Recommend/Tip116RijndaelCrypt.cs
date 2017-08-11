using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WinFormsAppFor157Recommend
{
    public class RijndaelCrypt
    {
        //缓冲区大小
        private int bufferSize = 128 * 1024;
        //密钥salt
        private byte[] salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };
        //初始化向量
        private byte[] iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

        //初始化并返回对称加密算法
        private SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }
        public string EncryptString(string input, string password)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = UTF32Encoding.Default.GetBytes(input);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.Flush();
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        public string DencryptString(string input, string password)
        {
            using (MemoryStream inputMemoryStream = new MemoryStream(Convert.FromBase64String(input)))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(inputMemoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    StreamReader sr = new StreamReader(cryptoStream);
                    return sr.ReadToEnd();
                }
            }
        }

        public void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.Open(outFile, FileMode.OpenOrCreate))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(outFileStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = new byte[bufferSize];
                    int readSize = -1;
                    while ((readSize = inFileStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cryptoStream.Write(bytes, 0, readSize);
                    }
                    cryptoStream.Flush();
                }
            }
        }

        public void DecryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.OpenWrite(outFile))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, salt))
            {
                algorithm.IV = iv;
                using (CryptoStream cryptoStream = new CryptoStream(inFileStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] bytes = new byte[bufferSize];
                    int readSize = -1;
                    int numReads = (int)(inFileStream.Length / bufferSize);
                    int slack = (int)(inFileStream.Length % bufferSize);
                    for (int i = 0; i < numReads; ++i)
                    {
                        readSize = cryptoStream.Read(bytes, 0, bytes.Length);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    if (slack > 0)
                    {
                        readSize = cryptoStream.Read(bytes, 0, (int)slack);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    outFileStream.Flush();
                }
            }
        }

    }
}
