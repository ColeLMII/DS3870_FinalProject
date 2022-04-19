using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace SwollenCoffee_WebServices
{
    public static class membership
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post","put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function, for membership methods.");

            string name = req.Query["name"]; 

            if(req.Method == HttpMethods.Get)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            } else if(req.Method == HttpMethods.Post)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            } else if(req.Method == HttpMethods.Put)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            }

            
            return new OkObjectResult("404: Error");
        }
    }
}
