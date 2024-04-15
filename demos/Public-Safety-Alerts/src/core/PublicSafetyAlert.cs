using System;

namespace PublicSafety
{
    public class PublicSafetyAlert
    {
        public string Id {get; set;}
        public DateTime UtcTime {get; set;}
        public string IssuingAuthority {get; set;}
        public string AlertType {get; set;}
        public string AffectedRegions {get; set;}

        public PublicSafetyAlert()
        {
            Id = "";
            UtcTime = DateTime.UtcNow;
            IssuingAuthority = "";
            AlertType = "";
            AffectedRegions = "";
        }

        public static PublicSafetyAlert Random()
        {
            //Random sample data
            string[] IssuingAuthorities = new string[]{"State Safety Commission (SSC)","Regional Emergency Response Agency (RERA)","Citizen Protection Bureau (CPB)","Municipal Safety Council (MSC)","National Crisis Management Center (NCMC)","Public Security Directorate (PSD)","Community Safety Task Force (CSTF)","Emergency Preparedness Authority (EPA)","Citizen Welfare and Security Office (CWSO)","Disaster Resilience Agency (DRA)","Safety and Crisis Intervention Division (SCID)","Homeland Security and Safety Directorate (HSSD)","Public Alert Coordination Unit (PACU)","City Risk Management Authority (CRMA)","Regional Safety and Security Board (RSSB)","Crisis Intervention and Preparedness Division (CIPD)","Safety Information Dissemination Center (SIDC)","Citizen Wellbeing and Protection Bureau (CWPB)","Emergency Response and Safety Task Group (ERSTG)","Local Security and Preparedness Office (LSPO)"};
            string[] AlertTypes = new string[]{"Severe Weather Warning","Public Health Emergency Alert","Civil Disturbance Advisory","Evacuation Order","Terror Threat Level Update","Infrastructure Failure Notification","Wildfire Danger Alert","Biological Hazard Warning","Air Quality Advisory","Power Outage Alert","Chemical Spill Alert","Shelter-in-Place Advisory","Transportation Disruption Warning","Missing Persons Alert","Cybersecurity Breach Notification"};
            string[] AffectedRegions = new string[]{"Eastwood City","Westville County","Northborough Township","Southdale District","Central Springs Municipality","Riverside Borough","Sunset Valley","Hillcrest Parish","Lakeside District","Pinegrove Township","Maplewood County","Meadowview City","Harbor Heights Region","Brookside Township","Sunflower County","Oakdale District","Crestwood Municipality","Willow Creek Township","Oceanview Parish","Silverlake Borough","Greenfield Township","Springdale City","Woodland County","Summitville Region","Falconridge District","Cedarwood Township","Golden Plains Municipality","Mountainview Parish","Sycamore Borough","Ivydale City"};

            PublicSafetyAlert psa = new PublicSafetyAlert();
            psa.Id = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
            psa.UtcTime = DateTime.UtcNow;
            psa.IssuingAuthority = RandomFromList(IssuingAuthorities);
            psa.AlertType = RandomFromList(AlertTypes);
            psa.AffectedRegions = RandomFromList(AffectedRegions);

            return psa;
        }

        private static string RandomFromList(string[] list)
        {
            Random r = new Random();
            return list[r.Next(0, list.Length)];
        }
    }
}