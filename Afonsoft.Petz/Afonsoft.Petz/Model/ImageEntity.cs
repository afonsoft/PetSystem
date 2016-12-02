using System;

namespace Afonsoft.Petz.Model
{
    /// <summary>
    /// Classe da imagem em Byte
    /// </summary>
    [Serializable]
    public class ImageEntity
    {
        public int Id { get; set; }
        public byte[] Binary { get; set; }
        public string Url { get; set; }
    }
}