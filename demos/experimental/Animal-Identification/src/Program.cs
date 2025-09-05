using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimalID
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IdentificationOutput io = IdentificationOutput.IdentifyAsync(@"C:\Users\timh\Downloads\Sample Photos from Tien\327\327\IMG_0001.JPG").Result;
            
        }

        
    }
}