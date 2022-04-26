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
<<<<<<< Updated upstream
                    string strMembershipID = req.Query["strMembershipID"];

                    string strQuery = "SELECT * FROM dbo.tblCustomers WHERE MembershipID = @MembershipID";
=======
                    string strSessionID = req.Query["strSessionID"];
                    DataSet dsLocations = new DataSet();
                    string strQuery = "SELECT * FROM dbo.tblCustomers WHERE Email = @Email";
>>>>>>> Stashed changes
                    // Put your using 
                    using (SqlConnection conTasks = new SqlConnection(strTasksConnectionString))
                    using (SqlCommand comUser = new SqlCommand(strQuery, conTasks))
                    {
                        SqlParameter parMemID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                        parMemID.Value = strMembershipID;
                        comUser.Parameters.Add(parMemID);

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
<<<<<<< Updated upstream
                    
                    //insert into customer
                    string strQuery = "insert into dbo.tblCustomers (Email, FirstName, LastName, DateofBirth, MembershipID, PreferredLocation) values(@... ,)";
                    
=======
                   
                    string strQuery = "INSERT INTO dbo.tblUsers (Email, FirstName, LastName, Password, Address, Address2, PhoneNumber, DateOfBirth) VALUES (@Email, @FirstName, @LastName, @Address, @PhoneNumber, @DateOfBirth, 'ACTIVE')";

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
                        SqlParameter parDOB = new SqlParameter("DateofBirth", SqlDbType.VarChar);
                        parDOB.Value = strDateOfBirth;
                        comNewUser.Parameters.Add(parDOB);
=======
                        SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                        parPassword.Value = strPassword;
                        comNewUser.Parameters.Add(parPassword);

                        SqlParameter parAddress1 = new SqlParameter("Address1", SqlDbType.VarChar);
                        parAddress1.Value = strAddress1;
                        comNewUser.Parameters.Add(parAddress1);

                        SqlParameter parAddress2 = new SqlParameter("Address1", SqlDbType.VarChar);
                        parAddress2.Value = strAddress2;
                        comNewUser.Parameters.Add(parAddress2);

                        SqlParameter parCity = new SqlParameter("City", SqlDbType.VarChar);
                        parCity.Value = strCity;
                        comNewUser.Parameters.Add(parCity);

                        SqlParameter parState = new SqlParameter("State", SqlDbType.VarChar);
                        parState.Value = strState;
                        comNewUser.Parameters.Add(parState);

                        SqlParameter parZIP = new SqlParameter("ZIP", SqlDbType.VarChar);
                        parZIP.Value = strZIP;
                        comNewUser.Parameters.Add(parZIP);
>>>>>>> Stashed changes

                        SqlParameter parMemID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                        parMemID.Value = strMembershipID;
                        comNewUser.Parameters.Add(parMemID);
                        
                        //return new OkObjectResult("User Added");
                    }

<<<<<<< Updated upstream
                    strQuery = "insert into dbo.tblSessions (SessionID, Email, StartDateTime, LastUsedDateTime,Type) values (@... ,)";
                    using (SqlConnection conNewUser = new SqlConnection(strTasksConnectionString))
                    using (SqlCommand comNewUser = new SqlCommand(strQuery, conNewUser))
                    {
                        SqlParameter parMemID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                        parMemID.Value = strMembershipID;
                        comNewUser.Parameters.Add(parMemID);
=======
                        SqlParameter parDOB = new SqlParameter("Address", SqlDbType.VarChar);
                        parDOB.Value = strDateOfBirth;
                        comNewUser.Parameters.Add(parDOB);
>>>>>>> Stashed changes

                        SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                        parEmail.Value = strEmail;
                        comNewUser.Parameters.Add(parEmail);
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
                    strTasksConnectionString = ;
                    SqlConnection conSwollenCoffee = new SqlConnection(strTasksConnectionString);
                    string strEmail = req.Query["strEmail"];
                    string strPassword = req.Query["strPassword"];

                    string strquery = "select * from dbo.tblUsers where UPPER(Email) = UPPER(@Email) and Password = @Password";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strquery, conSwollenCoffee))
                    {
                        //insert par statements

                        SqlDataAdapter daUsers = new SqlDataAdapter(comUsers);
                        daUsers.Fill(dsUsers);

                        if(dsUsers.Table[0].rows.Count > 0)
                        {
                            strquery= "insert into dbo.tblSession values"
                        }
                    }

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
                        using (SqlConnection conPurch = new SqlConnection(strTasksConnectionString))
                        using (SqlCommand comPurch = new SqlCommand(strQuery, conPurch))
                        {
                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            //parEmail.Value = strEmail;
                            comPurch.Parameters.Add(parEmail);
                            SqlDataAdapter daUsers = new SqlDataAdapter(comPurch);
                            daUsers.Fill(dsPurchases);
                            return new OkObjectResult(dsPurchases.Tables[0]);
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
