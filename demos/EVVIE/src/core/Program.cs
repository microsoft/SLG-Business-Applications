using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string imgpath = @"C:\Users\timh\Downloads\plate.jpg";
            byte[] imgbytes = File.ReadAllBytes(imgpath);
            string b64 = "data:image/jpeg;base64," + Convert.ToBase64String(imgbytes);
            System.IO.File.WriteAllText(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\EVVIE\src\core\B64.txt", b64);
        }
    }
}