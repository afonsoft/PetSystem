using System;
using System.Security.Cryptography;
using System.Text;

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
        /// <param name="decryptedMessage">String a ser criptografar</param>
        /// <returns>String Criptografar</returns>
        public static string Encryptor(string decryptedMessage)
        {
            if (decryptedMessage == null) throw new ArgumentNullException("decryptedMessage");

            return Encryptor(decryptedMessage, "AbCdEfGh");
        }
        /// <summary>
        /// Criptografar uma string
        /// </summary>
        /// <param name="decryptedMessage">String a ser criptografar</param>
        /// <param name="key">Chave usada para criptografar </param>
        /// <returns>String Criptografar</returns>
        public static string Encryptor(string decryptedMessage,string key)
        {
            if (decryptedMessage == null) throw new ArgumentNullException("decryptedMessage");
            if (key == null) throw new ArgumentNullException("key");

            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(key);
            tripleDes.Key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            byte[] data = Encoding.ASCII.GetBytes(decryptedMessage);
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
        /// <param name="encryptedMessage">String Criptografado</param>
        /// <returns>String desriptografar</returns>
        public static string Decryptor(string encryptedMessage)
        {
            if (encryptedMessage == null) throw new ArgumentNullException("encryptedMessage");

            return Decryptor(encryptedMessage, "AbCdEfGh");
        }
        /// <summary>
        /// Descriptografar uma string
        /// </summary>
        /// <param name="encryptedMessage">String Criptografado</param>
        /// <param name="key">Chave usada para Descriptografar </param>
        /// <returns>String desriptografar</returns>
        public static string Decryptor(string encryptedMessage, string key)
        {
            if (encryptedMessage == null) throw new ArgumentNullException("encryptedMessage");
            if (key == null) throw new ArgumentNullException("key");

            TripleDES tripleDes = TripleDES.Create();
            tripleDes.IV = Encoding.ASCII.GetBytes(key);
            tripleDes.Key = Encoding.ASCII.GetBytes("passwordDR0wSS@P6660juht");
            tripleDes.Mode = CipherMode.CBC;
            tripleDes.Padding = PaddingMode.Zeros;
            ICryptoTransform crypto = tripleDes.CreateDecryptor();
            byte[] decodedInput = StringToByteArray(encryptedMessage);
            byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);
            return Encoding.ASCII.GetString(decryptedBytes);
        }
        private static byte[] StringToByteArray(string hex)
        {
            if (hex == null) throw new ArgumentNullException("hex");

            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private static string ByteArrayToString(byte[] bin)
        {
            if (bin == null) throw new ArgumentNullException("bin");

            string hex = BitConverter.ToString(bin);
            return hex.Replace("-", "");
        }
    }
}