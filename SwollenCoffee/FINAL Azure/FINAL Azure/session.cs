using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

//Create session w/ username and password, return sessionID or error message (POST)
//Update session w/ a Last Used Date and Time based on sessionID (PUT)
//Delete session w/ provided sessionID (DELETE)
//Return session details when provided sessionID (GET)
namespace FINAL_Azure
{
    public static class session
    {
        public class UserLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public UserLogin (string strEmail, string strPassword)
            {
                Email = strEmail;
                Password = strPassword;
            }
        }
        [FunctionName("session")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post", "put", "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult("TEST");
        }
    }
}
