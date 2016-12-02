using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Zip.Streams.DeflaterOutputStream ZipStream = new Zip.Streams.DeflaterOutputStream(memoryStream, new Zip.Deflater(Zip.Deflater.BEST_COMPRESSION), 131072))
                    {
                        ZipStream.Write(data, 0, data.Length);
                        ZipStream.Close();
                    }
                    return memoryStream.ToArray();
                }
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

                using (Zip.Streams.InflaterInputStream ZipStream = new Zip.Streams.InflaterInputStream(new MemoryStream(data)))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        int size;
                        while (true)
                        {
                            size = ZipStream.Read(buffer, 0, buffer.Length);
                            if (size > 0)
                                stream.Write(buffer, 0, size);
                            else
                                break;
                        }
                        ZipStream.Close();
                        return stream.ToArray();
                    }
                }
            }
            catch
            {
                return data;
            }
        }
    }
}