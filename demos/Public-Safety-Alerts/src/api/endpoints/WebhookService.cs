using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PublicSafety;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace PublicSafetyAPI
{
    public class WebhookService
    {

        [Function("subscribe")]
        public async Task<HttpResponseData> Subscribe([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string errmsg = "";
            try
            {
                //Get the url param from the body
                StreamReader sr = new StreamReader(req.Body);
                string body = await sr.ReadToEndAsync();

                //Parse as JSON
                JObject jo = JObject.Parse(body);
                JProperty? prop_url = jo.Property("url");
                if (prop_url != null)
                {
                    string url = prop_url.Value.ToString();
                    WebhookSubscription sub = new WebhookSubscription();
                    sub.Endpoint = url;

                    //Subscribe!
                    MockDatabase db = new MockDatabase();
                    await db.AddWebhookSubscriptionAsync(sub);

                    //Return success
                    HttpResponseData success = req.CreateResponse();
                    success.StatusCode = System.Net.HttpStatusCode.Created;
                    success.Headers.Add("Location", "https://publicsafetyalerts.azurewebsites.net/unsubscribe/" + sub.Id);
                    return success;
                }
                else
                {
                    HttpResponseData resp = req.CreateResponse();
                    resp.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    await resp.WriteStringAsync("You need to include the \"url\" property in your body.");
                    return resp;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }

            HttpResponseData fresp = req.CreateResponse();
            fresp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            await fresp.WriteStringAsync("Failure! Msg: " + errmsg);
            return fresp;
        }

        [Function("unsubscribe")]
        public async Task<HttpResponseData> Unsubscribe([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "unsubscribe/{subid}")] HttpRequestData req, string subid)
        {
            string errmsg = "";
            try
            {
                //Handle bad scenario
                if (subid == null || subid == "")
                {
                    HttpResponseData badreq = req.CreateResponse();
                    badreq.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    await badreq.WriteStringAsync("You did not specify your webhook subscription ID properly in the URL path!");
                    return badreq;
                }

                //Is it "all"? If the ID is "all", clear all
                if (subid.ToLower().Trim() == "all")
                {
                    MockDatabase dbc = new MockDatabase();
                    await dbc.ClearWebhookSubscriptionsAsync();

                    HttpResponseData respc = req.CreateResponse();
                    respc.StatusCode = System.Net.HttpStatusCode.OK;
                    return respc;
                }

                //Check that there IS one with this
                MockDatabase db = new MockDatabase();
                WebhookSubscription[] subs = await db.DownloadWebhookSubscriptionsAsync();
                bool HaveSubWithThisId = false;
                foreach (WebhookSubscription sub in subs)
                {
                    if (sub.Id.ToLower().Trim() == subid.ToLower().Trim())
                    {
                        HaveSubWithThisId = true;
                    }
                }

                //If we do not have a sub with this Id, return not found
                if (!HaveSubWithThisId)
                {
                    HttpResponseData nf = req.CreateResponse();
                    nf.StatusCode = System.Net.HttpStatusCode.NotFound;
                    await nf.WriteStringAsync("A webhook subscription with ID '" + subid + "' was not found.");
                    return nf;
                }

                //Unsub
                await db.RemoveWebhookSubscriptionAsync(subid);

                //Return
                HttpResponseData resp = req.CreateResponse();
                resp.StatusCode = System.Net.HttpStatusCode.OK; //webhook unsubscribed!
                return resp;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }

            HttpResponseData fresp = req.CreateResponse();
            fresp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            await fresp.WriteStringAsync("Failure! Msg: " + errmsg);
            return fresp;
        }


    }
}