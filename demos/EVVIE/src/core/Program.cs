using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ResizeAndB64ConvertBulk(@"C:\Users\timh\Downloads\CURTIS", 0.25f);
        }

        public static void ResizeAndB64ConvertBulk(string folder, float resize = 1.0f)
        {
            string[] files = System.IO.Directory.GetFiles(folder);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string nameNoExtension = Path.GetFileNameWithoutExtension(file);

                //Convert to base64
                string b64 = ImageToBase64(file, resize);

                //Save
                string newname = nameNoExtension + ".txt";
                string newpath = Path.Combine(folder, newname);
                System.IO.File.WriteAllText(newpath, b64);

                //Print status!
                Console.WriteLine("Converted '" + name + "' to '" + Path.GetFileName(newpath) + "'");
            }


        }

        public static string ImageToBase64(string path, float resize = 1.0f)
        {
            //Perform resize
            Image i = Image.FromFile(path);
            int NewWidth = Convert.ToInt32(Convert.ToSingle(i.Width) * resize);
            int NewHeight = Convert.ToInt32(Convert.ToSingle(i.Height) * resize);
            Bitmap Resized = new Bitmap(NewWidth, NewHeight);
            Graphics g = Graphics.FromImage(Resized);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(i, 0, 0, NewWidth, NewHeight);

            //Save to memory stream
            MemoryStream ms = new MemoryStream();
            Resized.Save(ms, ImageFormat.Jpeg);
            byte[] imgBytes = ms.ToArray();
            string b64 = "data:image/jpeg;base64," + Convert.ToBase64String(imgBytes);

            return b64;
        }

    }
}