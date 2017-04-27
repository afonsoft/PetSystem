using System.IO;
using Zip = ICSharpCode.SharpZipLib.Zip.Compression;

namespace Afonsoft.Petz.Library
{
    /// <summary>
    /// Classe para compactar e descompactar byte
    /// </summary>
    public static class Compressor
    {
        /// <summary>
        /// Compress a byte
        /// </summary>
        public static byte[] Compress(byte[] data)
        {
            try
            {
                if (data == null)
                    return null;

                var memoryStream = new MemoryStream();
                using (var zipStream = new Zip.Streams.DeflaterOutputStream(memoryStream, new Zip.Deflater(Zip.Deflater.BEST_COMPRESSION), 131072))
                {
                    zipStream.Write(data, 0, data.Length);
                    data = memoryStream.ToArray();
                }
                return data;
            }
            catch
            {
                return data;
            }
        }

        /// <summary>
        /// Decompress a byte
        /// </summary>
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                if (data == null)
                    return null;

                using (var zipStream = new Zip.Streams.InflaterInputStream(new MemoryStream(data)))
                {
                    using (var stream = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            var size = zipStream.Read(buffer, 0, buffer.Length);
                            if (size > 0)
                                stream.Write(buffer, 0, size);
                            else
                                break;
                        }
                        data = stream.ToArray();
                    }
                }
                return data;
            }
            catch
            {
                return data;
            }
        }
    }
}

