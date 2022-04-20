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
using System.Data.SqlClient;
using System.Data;

namespace SwollenCoffee_WebServices
{
    public static class session
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string strTaskConnection = 
            if (req.Method == HttpMethods.Get)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            }
            else if (req.Method == HttpMethods.Post)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            }
            else if (req.Method == HttpMethods.Put)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return new OkObjectResult(ex);
                }
            } else if (req.Method == HttpMethods.Delete)
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
