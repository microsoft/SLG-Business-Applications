using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            byte[] content = System.IO.File.ReadAllBytes(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\Vehicle-Inspections-AI\damage3.jpg");
            string b64 = Convert.ToBase64String(content);
            b64 = "data:image/jpeg;base64," + b64;

            byte[] content2 = System.IO.File.ReadAllBytes(@"C:\Users\timh\Downloads\SLG-Business-Applications\demos\Vehicle-Inspections-AI\damage1.jpg");
            string b642 = Convert.ToBase64String(content);
            b642 = "data:image/jpeg;base64," + b642;

            //string inspection = VehicleInspectionGPT.CallAsync(b64).Result.ToString();
            //Console.WriteLine(inspection);
            

            VehicleInspection vi = VehicleInspectionGPT.InspectAsync(b64, b642).Result;
            Console.WriteLine(JsonConvert.SerializeObject(vi));

            
        

        
        }
    }
}