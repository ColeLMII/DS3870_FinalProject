using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

//create new member (POST)
//Update member (PUT)
//Return member's info when provided sessionID (GET)
namespace FINAL_Azure
{
    public static class membership
    {
        public class UserInfo
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime DateOfBirth { get; set; }
            public UserInfo(string strEmail, string strPassword, string strFirstName, string strLastName, string strAddress, string strNumber, DateTime BirthDay)//constructor
            {
                Email = strEmail;
                Password = strPassword;
                FirstName = strFirstName;
                LastName = strLastName;
                Address = strAddress;
                PhoneNumber = strNumber;
                DateOfBirth = BirthDay;
            }
            public bool VerifyPassword(string strPassword)
            {
                if (strPassword == Password)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post", "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult("TEST");
        }
    }
}
