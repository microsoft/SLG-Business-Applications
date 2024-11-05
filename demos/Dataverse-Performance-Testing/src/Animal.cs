using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataversePerformanceTesting
{
    public class Animal
    {
        public string Name {get; set;}
        public AnimalSpecies Species {get; set;}
        public DateTime DateOfBirth {get; set;}
        public int WeightPounds {get; set;}
        public float DailyFeedIntakePounds {get; set;}

        public Animal()
        {
            Name = "";
        }

        public JObject ForDataverseUpload()
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("timh_name", Name);
            ToReturn.Add("timh_species", (int)Species);
            ToReturn.Add("timh_dateofbirth", DateOfBirth.ToString());
            ToReturn.Add("timh_weightpounds", WeightPounds);
            ToReturn.Add("timh_dailyfeedintakepounds", DailyFeedIntakePounds);
            return ToReturn;
        }
    }
}