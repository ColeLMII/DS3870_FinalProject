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


namespace SQLIntegration
{
    public static class swollenCoffee
    {
        [FunctionName("swollenCoffee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = null)] HttpRequest req,
            ILogger log)
        {

            string strFunction = req.Query["function"];
            log.LogInformation("C# HTTP trigger function processed a request for " + strFunction);

            string strTasksConnectionString = @"Server=PCLABSQL01\COB_DS2,1436;Database=SwollenCoffee;User Id=student;Password=Mickey2020!;";
            string sessionID = req.Query["sessionID"];

            if (strFunction == "membership")
            {
                // do membership functions
                if (req.Method == HttpMethods.Get)
                {
                    //SELECT the membership info and return back that dataset table

                    //Variable info comes from the Query Parameters in the URL
                    //ie http://localhost:7071/swollenCoffee?function=membership&Email=bburchfield@tntech.edu
                    string strEmail = req.Query["Email"];

                    string strQuery = "SELECT * FROM dbo.tblCustomers WHERE Email = @Email";
                    // Put your using 
                }
                if (req.Method == HttpMethods.Post)
                {
                    //Variable info comes from the Query Parameters in the URL
                    //ie http://localhost:7071/swollenCoffee?function=membership&Firstname=Ben
                    string strFirstName = req.Query["Firstname"];
                    string strLastName = req.Query["Lastname"];
                    string strMembershipID = Guid.NewGuid().ToString();
                }
                if (req.Method == HttpMethods.Put)
                {
                    //Variable info comes from the Query Parameters in the URL
                    //ie http://localhost:7071/swollenCoffee?function=membership&Firstname=Ben
                    string strFirstName = req.Query["Firstname"];
                    string strLastName = req.Query["Lastname"];
                }

            }
            else if (strFunction == "location")
            {
                if (req.Method == HttpMethods.Get)
                {
                    string sessionID = req.Query["sessionID"];
                    DataSet dsLocations = new DataSet();
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
                            return new OkObjectResult(ex.Message.ToString());
                        }
                  
                }

            }
            else if (strFunction == "session")
            {
                if (req.Method == HttpMethods.Get)
                {

                }
                if (req.Method == HttpMethods.Post)
                {
                    string strSessionID = Guid.NewGuid().ToString();
                }
                if (req.Method == HttpMethods.Put)
                {

                }
                if (req.Method == HttpMethods.Delete)
                {

                }
            }
            else if (strFunction == "purchases")
            {
                if (req.Method == HttpMethods.Get)
                {

                }
            }
            else
            {
                return new OkObjectResult("Endpoint does not exist");
            }

            return new OkObjectResult("RESPONSE HERE");
        }
    }
}
