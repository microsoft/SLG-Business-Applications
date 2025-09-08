using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimalID
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IdentifyBatch().Wait();
        }

        public static async Task IdentifyBatch()
        {
            string folder = @"C:\Users\timh\Downloads\Sample Photos from Tien\327\327";
            string[] files = System.IO.Directory.GetFiles(folder);
            Console.WriteLine(files.Length.ToString("#,##0") + " files identified.");
            foreach (string file in files)
            {
                string file_name = System.IO.Path.GetFileName(file);
                Console.Write("Identifying '" + file_name + "'... ");
                IdentificationOutput io = await IdentificationOutput.IdentifyAsync(file);
                Console.Write(io.animals.Length.ToString() + " animals identified: ");

                if (io.animals.Length > 0)
                {
                    string animals = "";
                    foreach (AnimalID aid in io.animals)
                    {
                        animals = animals + aid.species + ", ";
                    }
                    animals = animals.Substring(0, animals.Length - 2);

                    Console.Write(animals);
                }

                //next line
                Console.WriteLine();
            }
        }

        
    }
}