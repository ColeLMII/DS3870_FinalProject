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

            if (strFunction == "membership")
            {
                // do membership functions
                if (req.Method == HttpMethods.Get)
                {
                    //SELECT the membership info and return back that dataset table

                    //Variable info comes from the Query Parameters in the URL
                    //ie http://localhost:7071/swollenCoffee?function=membership&Email=bburchfield@tntech.edu
                    string strSessionID = req.Query["strSessionID"];

                    string strQuery = "SELECT * FROM dbo.tblCustomers WHERE Email = @Email";
                    // Put your using 
                    using (SqlConnection conTasks = new SqlConnection(strTasksConnectionString))
                    using (SqlCommand comTasks = new SqlCommand(strQuery, conTasks))
                    {
                       
                        return new OkObjectResult(dsLocations);
                    }
                }
                if (req.Method == HttpMethods.Post)
                {
                    //Variable info comes from the Query Parameters in the URL
                    //ie http://localhost:7071/swollenCoffee?function=membership&Firstname=Ben
                    string strEmail = req.Query["strEmail"];
                    string strFirstName = req.Query["strFirstName"];
                    string strLastName = req.Query["strLastName"];
                    string strMembershipID = Guid.NewGuid().ToString();
                    string strPassword = req.Query["strPassword"];
                    string strAddress1 = req.Query["strAddress1"];
                    string strAddress2 = req.Query["strAddress2"];
                    string strCity = req.Query["strCity"];
                    string strState = req.Query["strstate"];
                    string strZIP = req.Query["strZIP"];
                    string strPhoneNumber = req.Query["strPhoneNumber"];
                    string strDateOfBirth = req.Query["strDateOfBirth"];
                   
                    string strQuery = "INSERT INTO dbo.tblUsers (Email, FirstName, LastName, Password, Address, PhoneNumber, DateOfBirth) VALUES (@Email, @FirstName, @LastName, @Address, @PhoneNumber, @DateOfBirth, 'ACTIVE')";

                    using (SqlConnection conNewUser = new SqlConnection(strTasksConnectionString))
                    using (SqlCommand comNewUser = new SqlCommand(strQuery, conNewUser))
                    {
                        SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                        parEmail.Value = strEmail;
                        comNewUser.Parameters.Add(parEmail);

                        SqlParameter parFirstname = new SqlParameter("FirstName", SqlDbType.VarChar);
                        parFirstname.Value = strFirstName;
                        comNewUser.Parameters.Add(parFirstname);

                        SqlParameter parLastName = new SqlParameter("LastName", SqlDbType.VarChar);
                        parLastName.Value = strLastName;
                        comNewUser.Parameters.Add(parLastName);

                        SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                        parPassword.Value = strPassword;
                        comNewUser.Parameters.Add(parPassword);

                        SqlParameter parAddress = new SqlParameter("Address", SqlDbType.VarChar);
                        parAddress.Value = strAddress;
                        comNewUser.Parameters.Add(parAddress);

                        SqlParameter parPhoneNumber = new SqlParameter("PhoneNumber", SqlDbType.VarChar);
                        parPhoneNumber.Value = strPhoneNumber;
                        comNewUser.Parameters.Add(parPhoneNumber);

                        SqlParameter parDOB = new SqlParameter("Address", SqlDbType.VarChar);
                        parAddress.Value = strDateOfBirth;
                        comNewUser.Parameters.Add(parDOB);

                        conNewUser.Open();
                        comNewUser.ExecuteNonQuery();
                        conNewUser.Close();
                        return new OkObjectResult("User Added");
                    }
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
                    string strSessionID = req.Query["SessionID"];
                    DataSet dsPurchases = new DataSet();
                    if (strSessionID == null || strSessionID == "")
                    {
                        string strQuery = "SELECT * FROM dbo.tblUsers";
                        using (SqlConnection conPurch = new SqlConnection(strTasksConnectionString))
                        using (SqlCommand comPurch = new SqlCommand(strQuery, conPurch))
                        {
                            SqlDataAdapter daUsers = new SqlDataAdapter(comPurch);
                            daUsers.Fill(dsPurchases);
                            return new OkObjectResult(dsPurchases.Tables[0]);
                        }
                    }
                    else
                    {
                        string strQuery = "SELECT * FROM dbo.tblUsers WHERE Email = @Email";
                        using (SqlConnection conPurch = new SqlConnection(str))
                        using (SqlCommand comDS3870 = new SqlCommand(strQuery, conDS3870))
                        {
                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comDS3870.Parameters.Add(parEmail);
                            SqlDataAdapter daUsers = new SqlDataAdapter(comDS3870);
                            daUsers.Fill(dsUsers);
                            return new OkObjectResult(dsUsers.Tables[0]);
                        }
                    }
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
