using System;

namespace PublicSafety
{
    public class WebhookSubscription
    {
        public string Id {get; set;}
        public string Endpoint {get; set;}

        public WebhookSubscription()
        {
            Id = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 10);
            Endpoint = "";
        }
    }
}