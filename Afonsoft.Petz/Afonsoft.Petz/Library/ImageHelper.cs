using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Afonsoft.Petz.Library
{
    /// <summary>
    /// Classe para trabalhar com imagens
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Converter os byte em Base64
        /// </summary>
        public static string byteArrayToBase64(byte[] byteArrayIn)
        {
            try
            {
                if (byteArrayIn == null)
                    return "";

                if (byteArrayIn.Length <= 0)
                    return "";

                return Convert.ToBase64String(byteArrayIn, 0, byteArrayIn.Length);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Converter a Base64 em Byte
        /// </summary>
        public static byte[] Base64TobyteArray(string base64String)
        {
            try
            {
                if (string.IsNullOrEmpty(base64String))
                    return null;
                return Convert.FromBase64String(base64String);
            }
            catch
            {
                return null;
            }
        }

        public static Image ByteToImage(byte[] imageBytes)
        {
            Image image;
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                image = Image.FromStream(ms, true, true);
            }
            return image;
        }

        public static Image Base64ToImage(string base64String)
        {
            Image image;
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                image = Image.FromStream(ms, true, true);    
            }
            return image;
        }

        public static Image DrawTextImage(String Text, Color textColor, Color backColor)
        {
            Font font = new Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            return DrawTextImage(Text, font, textColor, backColor, Size.Empty);
        }

        public static Image DrawTextImage(String Text, Font font, Color textColor, Color backColor)
        {
            return DrawTextImage(Text, font, textColor, backColor, Size.Empty);
        }
        public static Image DrawTextImage(String Text, Font font, Color textColor, Color backColor, Size minSize)
        {
            //first, create a dummy bitmap just to get a graphics object
            SizeF textSize;

            if (minSize.IsEmpty)
                minSize = new Size(50, 50);

            using (Image img = new Bitmap(1, 1))
            {
                using (Graphics drawing = Graphics.FromImage(img))
                {
                    //measure the string to see how big the image needs to be
                    textSize = drawing.MeasureString(Text, font);
                    if (!minSize.IsEmpty)
                    {
                        textSize.Width = textSize.Width > minSize.Width ? textSize.Width : minSize.Width;
                        textSize.Height = textSize.Height > minSize.Height ? textSize.Height : minSize.Height;
                    }
                }
            }

            //create a new image of the right size
            Image retImg = new Bitmap((int)textSize.Width, (int)textSize.Height);
            using (var drawing = Graphics.FromImage(retImg))
            {
                //paint the background
                drawing.Clear(backColor);
                drawing.SmoothingMode = SmoothingMode.HighQuality;
                drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //  <-- This is the correct value to use. ClearTypeGridFit is better yet!
                
                //create a brush for the text
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    drawing.DrawString(Text, font, textBrush, 0, 0);
                    drawing.Save();
                }
            }
            return retImg;
        }

        public static byte[] ConvertImage(byte[] imageBytes, ImageFormat format)
        {
            try
            {
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    using (var msConvert = new MemoryStream())
                    {
                        image.Save(msConvert, format);
                        return msConvert.ToArray();
                    }
                }
            }
            catch
            {
                return imageBytes;
            }
        }

        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}