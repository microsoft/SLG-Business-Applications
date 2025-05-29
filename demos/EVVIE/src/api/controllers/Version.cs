using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VehicleInspectionAI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace EVVIE_API
{
    [ApiController]
    [Route("version")]
    public class Version : ControllerBase
    {
        [HttpGet]
        public async Task Get()
        {
            Response.StatusCode = 200;
            Response.Headers["Content-Type"] = "text/plain";
            await Response.WriteAsync("0.2.0");
        }
    }
}