using System;

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
    }
}