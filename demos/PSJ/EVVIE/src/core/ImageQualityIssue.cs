using System;

namespace VehicleInspectionAI
{
    public class ImageQualityIssue
    {
        public string title { get; set; }
        public string description { get; set; }

        public ImageQualityIssue()
        {
            title = "";
            description = "";
        }
    }
}