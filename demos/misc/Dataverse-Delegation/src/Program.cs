using System;
using System.Text;
using System.Collections.Generic;
using TimHanewich.Dataverse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

namespace AnimalFarm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //UploadToDataverseAsync().Wait();
            UploadToSqlAsync().Wait();
            //DeleteAllDataverseAnimals().Wait();



            
        }

        public static void GenerateAnimals()
        {
            List<Animal> Animals = new List<Animal>();
            for (int t = 0; t < 30000; t++)
            {
                Console.WriteLine("Adding animal # " + t.ToString("#,##0..."));
                Animals.Add(Animal.Random());
            }

            //Serializing to file...
            System.IO.File.WriteAllText(@"C:\Users\timh\Downloads\AnimalFarm\animals.json", JsonConvert.SerializeObject(Animals.ToArray()));
        }


        #region "Dataverse"

        public static async Task UploadToDataverseAsync()
        {
            string content = System.IO.File.ReadAllText(@"C:\Users\timh\Downloads\AnimalFarm\animals.json");
            Animal[]? records = JsonConvert.DeserializeObject<Animal[]>(content);
            if (records != null)
            {
                DataverseAuthenticator auth = DVAuth();
                await auth.GetAccessTokenAsync();
                
                DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);

                //Upload each
                for (int t = 0; t < records.Length; t++)
                {
                    float percent_done = Convert.ToSingle(t) / Convert.ToSingle(records.Length);
                    Console.Write("Uploading record # " + (t + 1).ToString("#,##0") +  " / " + records.Length.ToString("#,##0") + " to Dataverse (" + percent_done.ToString("#0.0%") + ")... ");

                    JObject jo = new JObject();
                    jo.Add("cr2fb_name", records[t].Name);
                    jo.Add("cr2fb_species", records[t].Species);
                    jo.Add("cr2fb_sex", records[t].Sex);
                    jo.Add("cr2fb_weight", records[t].Weight);

                    //Upload
                    await ds.CreateAsync("cr2fb_animals", jo);
                    Console.WriteLine("Success!");
                }
            } 
        }

        public static async Task DeleteAllDataverseAnimals()
        {
            DataverseAuthenticator auth = DVAuth();
            await auth.GetAccessTokenAsync();
            DataverseService ds = new DataverseService(auth.Resource, auth.AccessToken);

            //Create list to delete
            JArray ToDelete = new JArray(); //List of Dataverse animal records to delete
            JArray AllAnimals = await ds.ReadAsync("cr2fb_animals");
            foreach (JObject jo in AllAnimals)
            {
                ToDelete.Add(jo);
            }

            while (ToDelete.Count > 0)
            {
                Console.WriteLine(AllAnimals.Count.ToString() + " animal records found in current batch!");

                //Delete each one by one
                int t = 0;
                foreach (JObject a in ToDelete)
                {
                    float percent_done = Convert.ToSingle(t) / Convert.ToSingle(ToDelete.Count);
                    JProperty? prop = a.Property("cr2fb_animalid");
                    if (prop != null)
                    {
                        Guid id = Guid.Parse(prop.Value.ToString());
                        Console.Write("Deleting animal '" + id.ToString() + "' # " + t.ToString("#,##0") + " / " + AllAnimals.Count.ToString("#,##0") + " (" + percent_done.ToString("#0.0%") + ")... ");
                        await ds.DeleteAsync("cr2fb_animals", id);
                        Console.WriteLine("Deleted!");
                    }
                    t = t + 1;
                }


                //After we deleted all of those, try again
                ToDelete.Clear(); //Remove all
                AllAnimals = await ds.ReadAsync("cr2fb_animals");
                foreach (JObject jo in AllAnimals)
                {
                    ToDelete.Add(jo);
                }
            }

            
        }

        public static DataverseAuthenticator DVAuth()
        {
            DataverseAuthenticator auth = new DataverseAuthenticator();
            auth.Username = "<DATAVERSE USERNAME HERE>";
            auth.Password = "<DATAVERSE PASSWORD HERE>";
            auth.ClientId = Guid.Parse("51f81489-12ee-4a9e-aaae-a2591f45987d");
            auth.Resource = "<DATAVERSE URL HERE>";
            return auth;
        }
    
    
        #endregion
    
        #region "SQL"

        public static async Task UploadToSqlAsync()
        {
            string content = System.IO.File.ReadAllText(@"C:\Users\timh\Downloads\AnimalFarm\animals.json");
            Animal[]? records = JsonConvert.DeserializeObject<Animal[]>(content);
            if (records != null)
            {
                SqlConnection sqlcon = new SqlConnection("<SQL CONNECTION STRING HERE>");
                sqlcon.Open();
                for (int t = 0; t < records.Length; t++)
                {
                    float percent_done = Convert.ToSingle(t) / Convert.ToSingle(records.Length);
                    Console.Write("Inserting record # " + (t + 1).ToString("#,##0") +  " / " + records.Length.ToString("#,##0") + " to SQL (" + percent_done.ToString("#0.0%") + ")... ");

                    Animal a = records[t];

                    //Convert sex to value
                    int sexval = 0;
                    if (a.Sex == false)
                    {
                        sexval = 0;
                    }
                    else
                    {
                        sexval = 1;
                    }

                    
                    string cmdstr = "insert into Animals (Name, Species, Sex, Weight) values ('" + a.Name + "', '" + a.Species + "', " + sexval.ToString() + ", " + a.Weight.ToString() + ")";
                    SqlCommand cmd = new SqlCommand(cmdstr, sqlcon); 
                    await cmd.ExecuteNonQueryAsync();

                    Console.WriteLine("Inserted!");
                }
            }  
        }

        #endregion
    
    }
}