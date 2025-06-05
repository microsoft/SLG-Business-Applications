using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string folder = @"C:\Users\timh\Downloads\CURTIS";
            string[] files = System.IO.Directory.GetFiles(folder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                //Convert to base64
                byte[] content = System.IO.File.ReadAllBytes(file);
                string b64 = Convert.ToBase64String(content);
                b64 = "data:image/jpeg;base64," + b64;

                //Plop in drop
                System.IO.File.WriteAllText(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\EVVIE\src\core\DROP.txt", b64);

                Console.Write("File '" + name + "' base64 now dropped. Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}