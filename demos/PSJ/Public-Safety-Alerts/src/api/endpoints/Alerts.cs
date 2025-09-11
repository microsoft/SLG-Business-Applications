using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using PublicSafety;
using System.Net.Http;


namespace PublicSafetyAPI
{
    public class GetPublicSafetyAlert
    {
        [Function("alerts")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "alerts/{cmd?}")] HttpRequestData req, string? cmd)
        {

            string errmsg = "";
            try
            {

                if (req.Method.ToString().ToLower() == "get")
                {
                    //Is there a cmd?
                    if (cmd != null)
                    {
                        if (cmd != "")
                        {
                            Console.WriteLine("Command received: '" + cmd + "'");
                            if (cmd.Trim().ToLower() == "clear")
                            {
                                MockDatabase dbt = new MockDatabase();
                                await dbt.ClearPublicSafetyAlertsAsync();

                                HttpResponseData cresp = req.CreateResponse();
                                cresp.StatusCode = System.Net.HttpStatusCode.NoContent;
                                return cresp;
                            }
                            else
                            {
                                HttpResponseData breq = req.CreateResponse();
                                breq.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                await breq.WriteStringAsync("Command '" + cmd + "' not understood.");
                                return breq;
                            }
                        }
                    }

                    //Get data
                    MockDatabase db = new MockDatabase();
                    PublicSafetyAlert[] alerts = await db.DownloadPublicSafetyAlertsAsync();

                    HttpResponseData response = req.CreateResponse();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Headers.Add("Content-Type", "application/json");
                    await response.WriteStringAsync(JsonConvert.SerializeObject(alerts), System.Text.Encoding.UTF8);

                    return response;
                }
                else if (req.Method.ToLower().Trim() == "post") //Making a new record
                {
                    //Create random record
                    PublicSafetyAlert psa = PublicSafetyAlert.Random();

                    //Get body
                    StreamReader sr = new StreamReader(req.Body);
                    string jsontxt = await sr.ReadToEndAsync();

                    //If it isn't blank, it must be JSON. And if it is JSON, get the properties.
                    if (jsontxt != null && jsontxt != "")
                    {
                        //Parse
                        JObject jo;
                        try
                        {
                            jo = JObject.Parse(jsontxt);
                        }
                        catch
                        {
                            HttpResponseData bjson = req.CreateResponse();
                            bjson.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            await bjson.WriteStringAsync("JSON body not formatted properly!");
                            return bjson;
                        }

                        //Get individual properties
                        JProperty? prop_IssuingAuthority = jo.Property("IssuingAuthority");
                        JProperty? prop_AlertType = jo.Property("AlertType");
                        JProperty? prop_AffectedRegions = jo.Property("AffectedRegions");

                        //Plug in accordingly
                        if (prop_IssuingAuthority != null)
                        {
                            psa.IssuingAuthority = prop_IssuingAuthority.Value.ToString();
                        }
                        if (prop_AlertType != null)
                        {
                            psa.AlertType = prop_AlertType.Value.ToString();
                        }
                        if (prop_AffectedRegions != null)
                        {
                            psa.AffectedRegions = prop_AffectedRegions.Value.ToString();
                        }
                    }

                    //Add it to the DB
                    Console.WriteLine("Uploading to Database... ");
                    MockDatabase db = new MockDatabase();
                    await db.AddPublicSafetyAlertAsync(psa);
                    
                    //Get a list of webhook subscribers... and one by one, inform them of the new public safety alert!
                    Console.WriteLine("Getting list of subscribers... ");
                    WebhookSubscription[] subs = await db.DownloadWebhookSubscriptionsAsync();
                    Console.WriteLine(subs.Length.ToString() + " subscribers!");
                    foreach (WebhookSubscription sub in subs)
                    {
                        HttpRequestMessage hrm = new HttpRequestMessage();
                        hrm.Method = HttpMethod.Post;
                        hrm.RequestUri = new Uri(sub.Endpoint);
                        hrm.Content = new StringContent(JsonConvert.SerializeObject(psa), System.Text.Encoding.UTF8, "application/json");
                        HttpClient hc = new HttpClient();
                        Console.WriteLine("Notifying subscription '" + sub.Id + "' (" + sub.Endpoint + ")... ");

                        try
                        {
                            HttpResponseMessage subresp = await hc.SendAsync(hrm);
                            Console.WriteLine("'" + sub.Endpoint + "' accepted the message and returned code '" + subresp.StatusCode.ToString() + "'!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Message to '" + sub.Endpoint + "' failed! Msg: " + ex.Message);
                        }
                    }

                    //Assemble response body... purly for information purposes
                    JObject rjo = new JObject();
                    rjo.Add("newRecord", JObject.Parse(JsonConvert.SerializeObject(psa)));
                    rjo.Add("webhookSubscribersNotifiedCount", subs.Length);
                    rjo.Add("webhookSubscribersNotified", JArray.Parse(JsonConvert.SerializeObject(subs)));

                    //Say 201 created
                    HttpResponseData sresp = req.CreateResponse();
                    sresp.StatusCode = System.Net.HttpStatusCode.Created;
                    sresp.Headers.Add("Content-Type", "application/json");
                    await sresp.WriteStringAsync(rjo.ToString());
                    return sresp;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }

            HttpResponseData resp = req.CreateResponse();
            resp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            await resp.WriteStringAsync("Failure! Msg: " + errmsg);
            return resp;
            
        }
    }
}