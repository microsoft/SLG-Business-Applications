using System;
using Newtonsoft.Json.Linq;

namespace CopilotStudioAnalytics
{
    public class SystemUser
    {
        public string FullName {get; set;}
        public string SystemUserId {get; set;}
        public string Email {get; set;}

        public SystemUser()
        {
            FullName = "";
            SystemUserId = "";
            Email = "";
        }

        public static SystemUser[] Parse(JArray systemusers)
        {
            List<SystemUser> ToReturn = new List<SystemUser>();
            foreach (JObject systemuser in systemusers)
            {
                SystemUser su = new SystemUser();

                //Get FullName
                JProperty? prop_FullName = systemuser.Property("fullname");
                if (prop_FullName != null)
                {
                    su.FullName = prop_FullName.Value.ToString();
                }

                //Get System User ID
                JProperty? prop_systemuserid = systemuser.Property("systemuserid");
                if (prop_systemuserid != null)
                {
                    su.SystemUserId = prop_systemuserid.Value.ToString();
                }

                //Get Email
                JProperty? prop_internalemailaddress = systemuser.Property("internalemailaddress");
                if (prop_internalemailaddress != null)
                {
                    su.Email = prop_internalemailaddress.Value.ToString();
                }

                ToReturn.Add(su);
            }
            return ToReturn.ToArray();
        }
    }
}