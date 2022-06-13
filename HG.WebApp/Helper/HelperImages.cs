using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Helpers;


namespace HG.WebApp.Common
{
    public static class HelperImages
    {
        

        //The actual converting function  
        public static string GetImage(object img)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
        }


        public static void SaveAndResize(WebImage image, int width, int height, string filename, string filepath)
        {
            image.Resize(width, height);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string pathsave;
            if (System.IO.File.Exists(Path.Combine(filepath, filename)))
            {
                pathsave = Path.Combine(filepath, filename);
            }
            else
            {
                pathsave = Path.Combine(filepath, filename);
            }
            image.Save(pathsave);
        }

        public static void SaveAndResizeImage(WebImage image, int width, string filename, string filepath)
        {            
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            float nPercent = ((float)sourceWidth / (float)width);
            int destHeight = (int)(sourceHeight / nPercent);
            image.Resize(width, destHeight);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string pathsave;
            if (System.IO.File.Exists(Path.Combine(filepath, filename)))
            {
                pathsave = Path.Combine(filepath, filename);
            }
            else
            {
                pathsave = Path.Combine(filepath, filename);
            }
            image.Save(pathsave);
        }
        public static void SaveImage(WebImage image, string filename, string filepath)
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string pathsave;
            if (System.IO.File.Exists(Path.Combine(filepath, filename)))
            {
                pathsave = Path.Combine(filepath, filename);
            }
            else
            {
                pathsave = Path.Combine(filepath, filename);
            }
            image.Save(pathsave);
        }
    }
}
