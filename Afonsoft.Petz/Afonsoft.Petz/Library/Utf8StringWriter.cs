using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Afonsoft.Petz.Library
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }

    /// <summary>
    /// Serialize a Object to XML
    /// </summary>
    public static class Serialize
    {
        /// <summary>
        /// Serialize a Object to XML
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="toSerialize">Object</param>
        /// <returns>String XML</returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
                using (StringWriter textWriter = new Utf8StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }
            }
            catch
            {
                return "";
            }
        }
    }
}