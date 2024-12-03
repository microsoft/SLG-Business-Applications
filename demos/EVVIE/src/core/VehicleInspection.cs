using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VehicleInspectionAI
{
    public class VehicleInspection
    {
        public CarArea Area {get; set;}
        public string Description {get; set;}
        public int SeverityLevel {get; set;}

        public VehicleInspection()
        {
            Description = "";
            SeverityLevel = 0;
        }

        public JObject ToJson()
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("area", Convert.ToInt32(Area));
            ToReturn.Add("description", Description);
            ToReturn.Add("severityLevel", SeverityLevel);
            return ToReturn;
        }
    }
}