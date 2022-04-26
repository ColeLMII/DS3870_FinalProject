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
            string strTaskConnectionString = @"Server=PCLABSQL01\COB_DS2;Database=SwollenCoffee;User Id=student;Password=Mickey2020!;";
            SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString);
            string strFunction = req.Query["function"];
            log.LogInformation("C# HTTP trigger function processed a request for " + strFunction);

            if (strFunction == "membership")
            {
                if (req.Method == HttpMethods.Get)
                {
                    DataSet dsSessions = new DataSet();
                    string strMembershipID = req.Query["SessionID"];

                    string strQuery = "SELECT * FROM dbo.tblCustomers LEFT JOIN dbo.tblCustomerHomeLocations ON tblCustomers.Email = tblCustomerHomeLocations.Email LEFT JOIN dbo.tblPhone ON tblCustomers.MembershipID = tblPhone.Member LEFT JOIN dbo.tblAddress ON tblCustomers.MembershipID = tblAddress.Member WHERE tblCustomers.Email = (SELECT Email FROM dbo.tblSessions WHERE SessionID = @SessionID)";

                    using (conSwollenCoffee)
                    using (SqlCommand comUser = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                        parSessionID.Value = strMembershipID;
                        comUser.Parameters.Add(parSessionID);

                        SqlDataAdapter daSessions = new SqlDataAdapter(comUser);
                        daSessions.Fill(dsSessions);

                        return new OkObjectResult(dsSessions.Tables[0]);
                    }
                }
                if (req.Method == HttpMethods.Post)
                {
                    string strEmail = req.Query["Email"];
                    string strFirstName = req.Query["FirstName"];
                    string strLastName = req.Query["LastName"];
                    string strMembershipID = Guid.NewGuid().ToString();
                    string strPreferredLocation = req.Query["PreferredLocation"];
                    string strPassword = req.Query["Password"];
                    string strAddressID = Guid.NewGuid().ToString();
                    string strAddress1 = req.Query["Address1"];
                    string strAddress2 = req.Query["Address2"];
                    string strCity = req.Query["City"];
                    string strState = req.Query["State"];
                    string strZIP = req.Query["ZIP"];
                    string strPhoneID = Guid.NewGuid().ToString();
                    string strPhoneNumber = req.Query["PhoneNumber"];
                    string strDateOfBirth = req.Query["DateOfBirth"];
                    string strUpdateDateTime = req.Query["UpdateDateTime"];

                    //insert into customer
                    string strQuery = "insert into dbo.tblCustomers values (@Email, @FirstName, @LastName, @DOB, @MembershipID, @PreferredLocation";
                    
                    using (conSwollenCoffee)
                    using (SqlCommand comNewUser = new SqlCommand(strQuery, conSwollenCoffee))
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

                        SqlParameter parDOB = new SqlParameter("DateofBirth", SqlDbType.VarChar);
                        parDOB.Value = strDateOfBirth;
                        comNewUser.Parameters.Add(parDOB);

                        SqlParameter parMemID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                        parMemID.Value = strMembershipID;
                        comNewUser.Parameters.Add(parMemID);

                        SqlParameter parPreferredLocation = new SqlParameter("PreferredLocation", SqlDbType.VarChar);
                        parPreferredLocation.Value = strPreferredLocation;
                        comNewUser.Parameters.Add(parPreferredLocation);

                        conSwollenCoffee.Open();
                        comNewUser.ExecuteNonQuery();
                        conSwollenCoffee.Close();
                    }

                    strQuery = "insert into dbo.tblSessions (SessionID, Email, StartDateTime, LastUsedDateTime,Type) values (@SessionID, @Email, @StartDateTime, @LastUsedDateTime, @Type)";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                        parEmail.Value = strEmail;
                        comUsers.Parameters.Add(parEmail);

                        SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                        parPassword.Value = strMembershipID;
                        comUsers.Parameters.Add(parPassword);

                        conSwollenCoffee.Open();
                        comUsers.ExecuteNonQuery();
                        conSwollenCoffee.Close();
                    }
                }
                if (req.Method == HttpMethods.Put)
                {
                    string strEmail = req.Query["Email"];
                    string strFirstName = req.Query["Firstname"];
                    string strLastName = req.Query["Lastname"];
                    string strDateOfBirth = req.Query["DateOfBirth"];
                    string strMembershipID = Guid.NewGuid().ToString();
                    string strPreferredLocation = req.Query["PreferredLocation"];
                    string strPhoneID = Guid.NewGuid().ToString();
                    string strPhone = req.Query["TelephoneNumber"];
                    string strAddressID = Guid.NewGuid().ToString();
                    string strStreet1 = req.Query["Street1"];
                    string strStreet2 = req.Query["Street2"];
                    string strCity = req.Query["City"];
                    string strState = req.Query["State"];
                    string strZip = req.Query["Zip"];
                    string strUpdateDateTime = req.Query["UpdateDateTime"];
                }
            }
            else if (strFunction == "location")
            {
                if (req.Method == HttpMethods.Get)
                {

                    DataSet dsLocations = new DataSet();
                    string strQuery = "SELECT * FROM dbo.tblLocations";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlDataAdapter daLocations = new SqlDataAdapter(comUsers);
                        daLocations.Fill(dsLocations);

                        return new OkObjectResult(dsLocations.Tables[0]);
                    }
                }
            }
            else if (strFunction == "session")
            {
                if (req.Method == HttpMethods.Get)
                {
                    DataSet dsSessions = new DataSet();
                    string strSessionID = req.Query["SessionID"];
                    string strQuery = "SELECT * FROM dbo.tblSessions WHERE tblSessions.SessionID = @SessionID";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                        parSessionID.Value = strSessionID;
                        comUsers.Parameters.Add(parSessionID);

                        SqlDataAdapter daSessions = new SqlDataAdapter(comUsers);
                        daSessions.Fill(dsSessions);

                        return new OkObjectResult(dsSessions.Tables[0]);
                    }
                }
                if (req.Method == HttpMethods.Post)
                {
                    string strSessionID = Guid.NewGuid().ToString();
                    
                    string strEmail = req.Query["Email"];
                    string strPassword = req.Query["Password"];
                    DataSet dsUsers = new DataSet();
                    string strquery = "select * from dbo.tblUsers where UPPER(Email) = UPPER(@Email) and Password = @Password";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strquery, conSwollenCoffee))
                    {
                        SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                        parEmail.Value = strEmail;
                        comUsers.Parameters.Add(parEmail);

                        SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                        parPassword.Value = strPassword;
                        comUsers.Parameters.Add(parPassword);

                        SqlDataAdapter daUsers = new SqlDataAdapter(comUsers);
                        daUsers.Fill(dsUsers);
                    }
                    if(dsUsers.Tables[0].Rows.Count > 0)
                    {
                        string strQuery = "INSERT INTO dbo.tblSessions VALUES(@SessionID,UPPER(@Email),GETDATE(),GETDATE(),'Mobile')";
                        using (conSwollenCoffee)
                        using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = strSessionID;
                            comUsers.Parameters.Add(parSessionID);

                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comUsers.Parameters.Add(parEmail);

                            conSwollenCoffee.Open();
                            comUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                            return new OkObjectResult("{\"SessionID\":\"" + strSessionID + "\"}");
                        }
                    }
                }
                if (req.Method == HttpMethods.Put)
                {
                    string strSessionID = req.Query["SessionID"];
                    string strQuery = "UPDATE dbo.tblSessions SET LastUsedDateTime = GETDATE() WHERE tblSessions.SessionID = @SessionID";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                        parSessionID.Value = strSessionID;
                        comUsers.Parameters.Add(parSessionID);
                        conSwollenCoffee.Open();
                        comUsers.ExecuteNonQuery();
                        conSwollenCoffee.Close();
                        return new OkObjectResult("{\"Outcome\":\"Session Updated\"}");
                    }
                }
                if (req.Method == HttpMethods.Delete)
                {
                    string strSessionID = req.Query["SessionID"];
                    string strQuery = "DELETE FROM dbo.tblSessions WHERE tblSessions.SessionID = @SessionID";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                        parSessionID.Value = strSessionID;
                        comUsers.Parameters.Add(parSessionID);
                        conSwollenCoffee.Open();
                        comUsers.ExecuteNonQuery();
                        conSwollenCoffee.Close();
                        return new OkObjectResult("{\"Outcome\":\"Session Deleted\"}");
                    }
                }
            }
            else if (strFunction == "purchases")
            {
                if (req.Method == HttpMethods.Get)
                {
                    string strSessionID = req.Query["SessionID"];
                    DataSet dsPurchases = new DataSet();
                    string strQuery = "SELECT dbo.tblTransactions.*, dbo.tblTransactionItems.* FROM dbo.tblSessions LEFT JOIN dbo.tblTransactions ON tblSessions.Email = tblTransactions.Member LEFT JOIN tblTransactionItems ON tblTransactions.TransactionID = tblTransactionItems.Transaction WHERE tblSessions.SessionID = @SessionID";
                    using (conSwollenCoffee)
                    using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                    {
                        SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                        parSessionID.Value = strSessionID;
                        comUsers.Parameters.Add(parSessionID);
                        SqlDataAdapter daPurchases = new SqlDataAdapter(comUsers);
                        daPurchases.Fill(dsPurchases);
                        return new OkObjectResult(dsPurchases.Tables[0]);
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
