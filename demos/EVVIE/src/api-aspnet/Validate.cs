using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using VehicleInspectionAI;

namespace EVVIE_API
{
    [ApiController]
    [Route("validate")]
    public class Validate : ControllerBase
    {
        [HttpPost]
        public async Task Post()
        {
            Response.StatusCode = 200;
        }
    }
}