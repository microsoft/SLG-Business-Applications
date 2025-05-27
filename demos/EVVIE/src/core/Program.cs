using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string imgpath = @"C:\Users\timh\Downloads\damage2.jpg";
            byte[] imgbytes = File.ReadAllBytes(imgpath);
            string b64 = "data:image/jpeg;base64," + Convert.ToBase64String(imgbytes);

            string[] issues = ImageQualityValidationAgent.ValidateAsync(b64).Result;
            Console.WriteLine(JsonConvert.SerializeObject(issues));
        }
    }
}