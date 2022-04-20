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
    public static class location
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //connecting to the database
            string strTasksConnectionString = @"Server=PCLABSQL01\COB_DS2,1436;Database=SwollenCoffee;User Id=student;Password=Mickey2020!;";
            string name = req.Query["name"];
            DataSet dsLocations = new DataSet();

            if (req.Method == HttpMethods.Get)
            {
                try
                {
                    string strQuery = "select * from tblLocations;";
                    using (SqlConnection conTasks = new SqlConnection(strTasksConnectionString))
                    using (SqlCommand comTasks = new SqlCommand(strQuery, conTasks))
                    {
                        //must create a data adapter, all you to fill a dataset
                        SqlDataAdapter daTasks = new SqlDataAdapter(comTasks);
                        daTasks.Fill(dsLocations);

                        return new OkObjectResult(dsLocations);
                    }
                }
                catch (Exception ex)
                {
                    return new OkObjectResult(Ex.Message.ToString());
                }
            }

            return new OkObjectResult("Testing Error");
        }
    }
}
