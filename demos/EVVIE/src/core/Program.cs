using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            byte[] content = System.IO.File.ReadAllBytes(@"C:\Users\timh\Downloads\totaled.jpg");
            string b64 = Convert.ToBase64String(content);
            b64 = "data:image/jpeg;base64," + b64;
            System.IO.File.WriteAllText(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\Vehicle-Inspections-AI\b64.txt", b64);
        }
    }
}