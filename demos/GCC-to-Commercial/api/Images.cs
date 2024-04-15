using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CRUX
{
    public class Images
    {
        [Function("images")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "images/{id?}")] HttpRequestData req, string id)
        {

            if (req.Method.ToLower() == "post")
            {
                //Read the body
                StreamReader sr = new StreamReader(req.Body);
                string content = await sr.ReadToEndAsync();
                JObject jo = JObject.Parse(content);

                //Get the property
                JProperty? prop_content = jo.Property("content");
                if (prop_content != null)
                {

                    //Get bytes
                    byte[] bytes = new byte[]{};
                    string _content = prop_content.Value.ToString();
                    int loc1 = _content.IndexOf("base64");
                    if (loc1 != -1)
                    {
                        loc1 = _content.IndexOf(",", loc1 + 1);
                        string base64 = _content.Substring(loc1 + 1);
                        bytes = Convert.FromBase64String(base64);
                    }
                    else
                    {
                        bytes = Convert.FromBase64String(_content);
                    }

                    //Convert to memoryStream
                    MemoryStream ms = new MemoryStream(bytes);
                    ms.Position = 0;

                    //Upload
                    BlobServiceClient bsc = new BlobServiceClient(ConnectionStringProvider.GetConnectionString());
                    BlobContainerClient bcc = bsc.GetBlobContainerClient("images");
                    bcc.CreateIfNotExists();

                    //Name + upload
                    string name = Guid.NewGuid().ToString().Replace("-", "");
                    BlobClient bc = bcc.GetBlobClient(name);
                    await bc.UploadAsync(ms);
                    
                    //Return with the id
                    JObject response = new JObject();
                    response.Add("id", name);
                    HttpResponseData r = req.CreateResponse();
                    r.StatusCode = HttpStatusCode.Created;
                    r.Headers.Add("Content-Type", "application/json");
                    r.WriteString(response.ToString(Formatting.None));
                    return r;
                }
                else
                {
                    HttpResponseData ToReturn = req.CreateResponse();
                    ToReturn.StatusCode = HttpStatusCode.BadRequest;
                    ToReturn.WriteString("Property 'content' not included in body!");
                    return ToReturn;
                }
            }
            else if (req.Method.ToLower() == "get")
            {
                if (id == null || id == "")
                {
                    HttpResponseData ToReturn = req.CreateResponse();
                    ToReturn.StatusCode = HttpStatusCode.BadRequest;
                    ToReturn.WriteString("ID not provided in URL!");
                    return ToReturn;
                }

                //Upload
                BlobServiceClient bsc = new BlobServiceClient(ConnectionStringProvider.GetConnectionString());
                BlobContainerClient bcc = bsc.GetBlobContainerClient("images");
                BlobClient bc = bcc.GetBlobClient(id);
                
                if (bc.Exists() == false)
                {
                    HttpResponseData ToReturn = req.CreateResponse();
                    ToReturn.StatusCode = HttpStatusCode.BadRequest;
                    ToReturn.WriteString("Image with ID '" + id + "' does not exist!");
                    return ToReturn;
                }

                MemoryStream ms = new MemoryStream();
                await bc.DownloadToAsync(ms);
                ms.Position = 0;

                HttpResponseData response = req.CreateResponse();
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Add("Content-Type", "image/png");
                response.WriteBytes(ms.ToArray());
                return response;
            }
            else
            {
                HttpResponseData ToReturn = req.CreateResponse();
                ToReturn.StatusCode = HttpStatusCode.BadRequest;
                ToReturn.WriteString("Method '" + req.Method.ToString() + "' not supported.");
                return ToReturn;
            }

            
        }
    
        
    }
}