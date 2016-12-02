using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Afonsoft.Petz.Library
{
    /// <summary>
    /// Classe para Encryption text
    /// </summary>
    public static class Cryptographic
    {
        /// <summary>
        /// Criptografar uma string utilizando o TripleDES
        /// </summary>
        /// <param name="DecryptedMessage">String a ser criptografar</param>
        /// <returns>String Criptografar</returns>
        public static String Encryptor(String DecryptedMessage)
        {
            return Encryptor(DecryptedMessage, "AbCdEfGh");
        }
        /// <summary>
        /// Criptografar uma string
        /// </summary>
        /// <param name="DecryptedMessage">String a ser criptografar</param>
        /// <param name="key">Chave usada para criptografar </param>
        /// <returns>String Criptografar</returns>
        public static String Encryptor(String DecryptedMessage, String key)
        {
            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(key);
            tripleDes.Key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            byte[] data = Encoding.ASCII.GetBytes(DecryptedMessage);
            byte[] enc = new byte[0];
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;
            ICryptoTransform ict = tripleDes.CreateEncryptor();
            enc = ict.TransformFinalBlock(data, 0, data.Length);
            return ByteArrayToString(enc);

        }

        /// <summary>
        /// Descriptografar uma string
        /// </summary>
        /// <param name="EncryptedMessage">String Criptografado</param>
        /// <returns>String desriptografar</returns>
        public static String Decryptor(String EncryptedMessage)
        {
            return Decryptor(EncryptedMessage, "AbCdEfGh");
        }
        /// <summary>
        /// Descriptografar uma string
        /// </summary>
        /// <param name="EncryptedMessage">String Criptografado</param>
        /// <param name="key">Chave usada para Descriptografar </param>
        /// <returns>String desriptografar</returns>
        public static String Decryptor(String EncryptedMessage, String key)
        {
            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(key);
            tripleDes.Key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;
            ICryptoTransform crypto = tripleDes.CreateDecryptor();
            byte[] decodedInput = StringToByteArray(EncryptedMessage);
            byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);
            return Encoding.ASCII.GetString(decryptedBytes);
        }
        private static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private static string ByteArrayToString(byte[] bin)
        {
            string hex = BitConverter.ToString(bin);
            return hex.Replace("-", "");
        }
    }
}