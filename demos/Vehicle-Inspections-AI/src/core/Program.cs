using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            byte[] content = System.IO.File.ReadAllBytes(@"C:\Users\timh\Downloads\Vehicle Inspection AI\damage1.jpg");
            string b64 = Convert.ToBase64String(content);
            System.IO.File.WriteAllText(@"C:\Users\timh\Downloads\Vehicle Inspection AI\b64.txt", "data:image/jpeg;base64," + b64);
        }
    }
}