﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Afonsoft.Petz.Library
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns an enumerable collection of the specified type containing the substrings in this instance that are delimited by elements of a specified Char array
        /// </summary>
        /// <param name="text">The string.</param>
        /// <param name="separator">
        /// An array of Unicode characters that delimit the substrings in this instance, an empty array containing no delimiters, or null.
        /// </param>
        /// <typeparam name="T">
        /// The type of the elemnt to return in the collection, this type must implement IConvertible.
        /// </typeparam>
        /// <returns>
        /// An enumerable collection whose elements contain the substrings in this instance that are delimited by one or more characters in separator. 
        /// </returns>
        public static IEnumerable<T> SplitTo<T>(this string text, params char[] separator) where T : IConvertible
        {
            foreach (var s in text.Split(separator, StringSplitOptions.None))
                yield return (T)Convert.ChangeType(s, typeof(T));
        }

        /// <summary>
        /// string.Format
        /// </summary>
        public static string Format(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new[] { arg }.Concat(additionalArgs).ToArray());
            }
        }

        /// <summary>
        /// Converts a string into a "SecureString"
        /// </summary>
        /// <param name="text">Input String</param>
        public static System.Security.SecureString ToSecureString(this String text)
        {
            System.Security.SecureString secureString = new System.Security.SecureString();
            foreach (Char c in text)
                secureString.AppendChar(c);

            return secureString;
        }

        /// <summary>
        /// Verificar se é numerico o valor da string
        /// </summary>
        public static bool IsNumeric(this string text)
        {
            long retNum;
            return long.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out retNum);
        }

        /// <summary>
        /// Remover os acentos do texto
        /// </summary>
        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString().Trim();
        }
        /// <summary>
        /// Remover os caracteres especiais do texto
        /// </summary>
        public static string RemoveSpecialCharacters(this string value)
        {
            value = Regex.Replace(value, "[«»\u201C\u201D\u201E\u201F\u2033\u2036]", "");
            value = Regex.Replace(value, "[èëêð]", "e");
            value = Regex.Replace(value, "[ÈËÊ]", "E");
            value = Regex.Replace(value, "[àâä]", "a");
            value = Regex.Replace(value, "[ÀÂÄÅ]", "A");
            value = Regex.Replace(value, "[ÙÛÜ]", "U");
            value = Regex.Replace(value, "[úûüµ]", "u");
            value = Regex.Replace(value, "[òöø]", "o");
            value = Regex.Replace(value, "[ÒÖØ]", "O");
            value = Regex.Replace(value, "[ìîï]", "i");
            value = Regex.Replace(value, "[ÌÎÏ]", "I");
            value = Regex.Replace(value, "[š]", "s");
            value = Regex.Replace(value, "[Š]", "S");
            value = Regex.Replace(value, "[ñ]", "n");
            value = Regex.Replace(value, "[Ñ]", "N");
            value = Regex.Replace(value, "[ÿ]", "y");
            value = Regex.Replace(value, "[Ÿ]", "Y");
            value = Regex.Replace(value, "[ž]", "z");
            value = Regex.Replace(value, "[Ž]", "Z");
            value = Regex.Replace(value, "[Ð]", "D");
            value = Regex.Replace(value, "[œ]", "oe");
            value = Regex.Replace(value, "[Œ]", "Oe");
            value = Regex.Replace(value, "[\"]", "");
            value = Regex.Replace(value, "[\t]", "");
            value = Regex.Replace(value, "[\n]", "");
            value = Regex.Replace(value, "[\r]", "");
            value = Regex.Replace(value, "[\u2026]", "...");
            value = Regex.Replace(value, Environment.NewLine, "");
            value = Regex.Replace(value, "��", "");
            value = Regex.Replace(value, "�", "");
            value = Regex.Replace(value, "[?]", "");
            return value.Trim();
        }
    }
}