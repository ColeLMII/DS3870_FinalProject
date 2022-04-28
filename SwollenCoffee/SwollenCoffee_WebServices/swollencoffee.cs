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
            
            string strFunction = req.Query["function"];
            log.LogInformation("C# HTTP trigger function processed a request for " + strFunction);

            try
            {
                if (strFunction == "membership")
                {
                    if (req.Method == HttpMethods.Get)
                    {
                        DataSet dsSessions = new DataSet();
                        string strMembershipID = req.Query["SessionID"];

                        string strQuery = "SELECT * FROM dbo.tblCustomers LEFT JOIN dbo.tblCustomerHomeLocations ON tblCustomers.Email = tblCustomerHomeLocations.Email LEFT JOIN dbo.tblPhone ON tblCustomers.MembershipID = tblPhone.Member LEFT JOIN dbo.tblAddress ON tblCustomers.MembershipID = tblAddress.Member WHERE tblCustomers.Email = (SELECT Email FROM dbo.tblSessions WHERE SessionID = @SessionID)";

                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
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
                        string strDateOfBirth = req.Query["DOB"];
                        //string strUpdateDateTime = req.Query["UpdateDateTime"];

                        //insert into customer
                        string strQuery = "insert into dbo.tblCustomers values (@Email, @FirstName, @LastName, @DOB, @MembershipID, @PreferredLocation)";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
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

                            SqlParameter parDOB = new SqlParameter("DOB", SqlDbType.VarChar);
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

                        //insert into phone
                        string strQuery1 = "insert into dbo.tblPhone VALUES (@PhoneID, @NationCode, @AreaCode, @TelephoneNumber, @MembershipID)";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsersPhone = new SqlCommand(strQuery1, conSwollenCoffee))
                        {
                            SqlParameter parPhoneID = new SqlParameter("PhoneID", SqlDbType.VarChar);
                            parPhoneID.Value = strPhoneID;
                            comUsersPhone.Parameters.Add(parPhoneID);

                            SqlParameter parNationCode = new SqlParameter("NationCode", SqlDbType.VarChar);
                            parNationCode.Value = "1";
                            comUsersPhone.Parameters.Add(parNationCode);

                            SqlParameter parAreaCode = new SqlParameter("AreaCode", SqlDbType.VarChar);
                            parAreaCode.Value = strPhoneNumber.Substring(0, 3);
                            comUsersPhone.Parameters.Add(parAreaCode);

                            SqlParameter parTelephoneNumber = new SqlParameter("TelephoneNumber", SqlDbType.VarChar);
                            parTelephoneNumber.Value = strPhoneNumber.Substring(3);
                            comUsersPhone.Parameters.Add(parTelephoneNumber);

                            SqlParameter parMembershipID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                            parMembershipID.Value = strMembershipID;
                            comUsersPhone.Parameters.Add(parMembershipID);

                            conSwollenCoffee.Open();
                            comUsersPhone.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        string strQuery2 = "INSERT INTO dbo.tblUsers VALUES(UPPER(@Email),'User',@Password,GETDATE(),GETDATE(),1)";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comINSUsers = new SqlCommand(strQuery2, conSwollenCoffee))
                        {
                            SqlParameter parEmail2 = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail2.Value = strEmail;
                            comINSUsers.Parameters.Add(parEmail2);

                            SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                            parPassword.Value = strPassword;
                            comINSUsers.Parameters.Add(parPassword);


                            conSwollenCoffee.Open();
                            comINSUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        //insert into Address
                        string strQuery3 = "INSERT INTO dbo.tblAddress VALUES (@AddressID, @Address1, @Address2, @City, @State, @ZIP, @Member)";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsersAddress = new SqlCommand(strQuery3, conSwollenCoffee))
                        {
                            SqlParameter parAddressID = new SqlParameter("AddressID", SqlDbType.VarChar);
                            parAddressID.Value = strAddressID;
                            comUsersAddress.Parameters.Add(parAddressID);

                            SqlParameter parAddress1 = new SqlParameter("Address1", SqlDbType.VarChar);
                            parAddress1.Value = strAddress1;
                            comUsersAddress.Parameters.Add(parAddress1);

                            SqlParameter parAddress2 = new SqlParameter("Address2", SqlDbType.VarChar);
                            parAddress2.Value = strAddress2;
                            comUsersAddress.Parameters.Add(parAddress2);

                            SqlParameter parCity = new SqlParameter("City", SqlDbType.VarChar);
                            parCity.Value = strCity;
                            comUsersAddress.Parameters.Add(parCity);

                            SqlParameter parState = new SqlParameter("State", SqlDbType.VarChar);
                            parState.Value = strState;
                            comUsersAddress.Parameters.Add(parState);

                            SqlParameter parZIP = new SqlParameter("ZIP", SqlDbType.VarChar);
                            parZIP.Value = strZIP;
                            comUsersAddress.Parameters.Add(parZIP);

                            SqlParameter parMember = new SqlParameter("Member", SqlDbType.VarChar);
                            parMember.Value = strMembershipID;
                            comUsersAddress.Parameters.Add(parMember);

                            conSwollenCoffee.Open();
                            comUsersAddress.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        //insert into locations
                        string strQuery4 = "INSERT INTO dbo.tblCustomerHomeLocations VALUES (@Email, @LocationID, GETDATE())";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsersLocation = new SqlCommand(strQuery4, conSwollenCoffee))
                        {
                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comUsersLocation.Parameters.Add(parEmail);

                            SqlParameter parLocation = new SqlParameter("LocationID", SqlDbType.VarChar);
                            parLocation.Value = strPreferredLocation;
                            comUsersLocation.Parameters.Add(parLocation);

                            conSwollenCoffee.Open();
                            comUsersLocation.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        string streSessionID = Guid.NewGuid().ToString();

                        //insert into session
                        string strQuery5 = "INSERT INTO dbo.tblSessions VALUES (@SessionID, UPPER(@Email), GETDATE(), GETDATE(), 'Mobile')";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsersSession = new SqlCommand(strQuery5, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = streSessionID;
                            comUsersSession.Parameters.Add(parSessionID);

                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comUsersSession.Parameters.Add(parEmail);

                            conSwollenCoffee.Open();
                            comUsersSession.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }
                        return new OkObjectResult("{\"Outcome\":\"Success|" + strMembershipID + "|" + streSessionID + "\"}");
                    }
                    if (req.Method == HttpMethods.Put)
                    {
                        string strEmail = req.Query["Email"];
                        string strFirstName = req.Query["Firstname"];
                        string strLastName = req.Query["Lastname"];
                        string strDateOfBirth = req.Query["DateOfBirth"];
                        string strMembershipID = req.Query["MemebershipID"];
                        string strPreferredLocation = req.Query["PreferredLocation"];
                        string strPhoneID = req.Query["PhoneID"];
                        string strPhone = req.Query["TelephoneNumber"];
                        string strAddressID = req.Query["AddressID"];
                        string strAddress1 = req.Query["Address1"];
                        string strAddress2 = req.Query["Address2"];
                        string strCity = req.Query["City"];
                        string strState = req.Query["State"];
                        string strZip = req.Query["Zip"];

                        string strQuery = "UPDATE dbo.tblCustomers SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DOB, PreferredLocation = @PreferredLocation WHERE MembershipID = @MembershipID";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parFirstName = new SqlParameter("FirstName", SqlDbType.VarChar);
                            parFirstName.Value = strFirstName;
                            comUsers.Parameters.Add(parFirstName);

                            SqlParameter parLastName = new SqlParameter("LastName", SqlDbType.VarChar);
                            parLastName.Value = strLastName;
                            comUsers.Parameters.Add(parLastName);

                            SqlParameter parDOB = new SqlParameter("DOB", SqlDbType.VarChar);
                            parDOB.Value = strDateOfBirth;
                            comUsers.Parameters.Add(parDOB);

                            SqlParameter parPreferredLocation = new SqlParameter("PreferredLocation", SqlDbType.VarChar);
                            parPreferredLocation.Value = strPreferredLocation;
                            comUsers.Parameters.Add(parPreferredLocation);

                            SqlParameter parMembershipID = new SqlParameter("MembershipID", SqlDbType.VarChar);
                            parMembershipID.Value = strMembershipID;
                            comUsers.Parameters.Add(parMembershipID);

                            conSwollenCoffee.Open();
                            comUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        strQuery = "UPDATE dbo.tblPhone SET NationCode =@NationCode, AreaCOde = @AreaCode, TelephoneNumber = @TelephoneNumber WHERE PhoneID = @PhoneID";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                        {

                            SqlParameter parNationCode = new SqlParameter("NationCode", SqlDbType.VarChar);
                            parNationCode.Value = "1";
                            comUsers.Parameters.Add(parNationCode);

                            SqlParameter parAreaCode = new SqlParameter("AreaCode", SqlDbType.VarChar);
                            parAreaCode.Value = strPhone.Substring(0, 3);
                            comUsers.Parameters.Add(parAreaCode);

                            SqlParameter parTelephoneNumber = new SqlParameter("TelephoneNumber", SqlDbType.VarChar);
                            parTelephoneNumber.Value = strPhone.Substring(3);
                            comUsers.Parameters.Add(parTelephoneNumber);

                            SqlParameter parPhoneID = new SqlParameter("PhoneID", SqlDbType.VarChar);
                            parPhoneID.Value = strPhoneID;
                            comUsers.Parameters.Add(parPhoneID);

                            conSwollenCoffee.Open();
                            comUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        strQuery = "UPDATE dbo.tblAddress SET Street1 = @Address1,Address2 = @Address2,City = @City, State = @State,ZIP = @ZIP WHERE AddressID = @AddressID";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                        {


                            SqlParameter parAddress1 = new SqlParameter("Address1", SqlDbType.VarChar);
                            parAddress1.Value = strAddress1;
                            comUsers.Parameters.Add(parAddress1);

                            SqlParameter parAddress2 = new SqlParameter("Address2", SqlDbType.VarChar);
                            parAddress2.Value = strAddress2;
                            comUsers.Parameters.Add(parAddress2);

                            SqlParameter parCity = new SqlParameter("City", SqlDbType.VarChar);
                            parCity.Value = strCity;
                            comUsers.Parameters.Add(parCity);

                            SqlParameter parState = new SqlParameter("State", SqlDbType.VarChar);
                            parState.Value = strState;
                            comUsers.Parameters.Add(parState);

                            SqlParameter parZIP = new SqlParameter("ZIP", SqlDbType.VarChar);
                            parZIP.Value = strZip;
                            comUsers.Parameters.Add(parZIP);

                            SqlParameter parAddressID = new SqlParameter("AddressID", SqlDbType.VarChar);
                            parAddressID.Value = strAddressID;
                            comUsers.Parameters.Add(parAddressID);

                            conSwollenCoffee.Open();
                            comUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                        }

                        strQuery = "UPDATE dbo.tblCustomerHomeLocations SET LocationID = @LocationID, UpdateDateTime = GETDATE() WHERE Email = @Email";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUsers = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parLocation = new SqlParameter("LocationID", SqlDbType.VarChar);
                            parLocation.Value = strPreferredLocation;
                            comUsers.Parameters.Add(parLocation);

                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comUsers.Parameters.Add(parEmail);

                            conSwollenCoffee.Open();
                            comUsers.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                            return new OkObjectResult("{\"Outcome\":\"Success\"}");
                        }
                    }
                }
                else if (strFunction == "location")
                {
                    if (req.Method == HttpMethods.Get)
                    {

                        DataSet dsLocations = new DataSet();
                        string strQuery = "SELECT * FROM dbo.tblLocations";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
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
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comSelectSession = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = strSessionID;
                            comSelectSession.Parameters.Add(parSessionID);

                            SqlDataAdapter daSessions = new SqlDataAdapter(comSelectSession);
                            daSessions.Fill(dsSessions);

                            return new OkObjectResult(dsSessions.Tables[0]);
                        }
                    }
                    if (req.Method == HttpMethods.Post)
                    {
                        string strSessionID = Guid.NewGuid().ToString();
                        string strMembershipID = Guid.NewGuid().ToString();

                        string strEmail = req.Query["Email"];
                        string strPassword = req.Query["Password"];
                        DataSet dsUsers = new DataSet();
                        string strquery = "select * from dbo.tblUsers where UPPER(Email) = UPPER(@Email) and Password = @Password";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comSelect = new SqlCommand(strquery, conSwollenCoffee))
                        {
                            SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                            parEmail.Value = strEmail;
                            comSelect.Parameters.Add(parEmail);

                            SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar);
                            parPassword.Value = strPassword;
                            comSelect.Parameters.Add(parPassword);

                            SqlDataAdapter daUsers = new SqlDataAdapter(comSelect);
                            daUsers.Fill(dsUsers);
                        }
                        if (dsUsers.Tables[0].Rows.Count > 0)
                        {
                            string strQuery = "INSERT INTO dbo.tblSessions VALUES(@SessionID,UPPER(@Email),GETDATE(),GETDATE(),'Mobile')";
                            using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                            using (SqlCommand comInsert = new SqlCommand(strQuery, conSwollenCoffee))
                            {
                                SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                                parSessionID.Value = strSessionID;
                                comInsert.Parameters.Add(parSessionID);

                                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar);
                                parEmail.Value = strEmail;
                                comInsert.Parameters.Add(parEmail);

                                conSwollenCoffee.Open();
                                comInsert.ExecuteNonQuery();
                                conSwollenCoffee.Close();
                                
                                return new OkObjectResult("{\"SessionID\":\"" + strSessionID + "|" + strMembershipID + "\"}");
                            }
                        }
                        else
                        {
                            return new OkObjectResult("Login Failed");
                        }
                    }
                    if (req.Method == HttpMethods.Put)
                    {
                        string strSessionID = req.Query["SessionID"];
                        string strQuery = "UPDATE dbo.tblSessions SET LastUsedDateTime = GETDATE() WHERE tblSessions.SessionID = @SessionID";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comUpdate = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = strSessionID;
                            comUpdate.Parameters.Add(parSessionID);
                            conSwollenCoffee.Open();
                            comUpdate.ExecuteNonQuery();
                            conSwollenCoffee.Close();
                            return new OkObjectResult("{\"Outcome\":\"Session Updated\"}");
                        }
                    }
                    if (req.Method == HttpMethods.Delete)
                    {
                        string strSessionID = req.Query["SessionID"];
                        string strQuery = "DELETE FROM dbo.tblSessions WHERE tblSessions.SessionID = @SessionID";
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comDelete = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = strSessionID;
                            comDelete.Parameters.Add(parSessionID);
                            conSwollenCoffee.Open();
                            comDelete.ExecuteNonQuery();
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
                        using (SqlConnection conSwollenCoffee = new SqlConnection(strTaskConnectionString))
                        using (SqlCommand comSession = new SqlCommand(strQuery, conSwollenCoffee))
                        {
                            SqlParameter parSessionID = new SqlParameter("SessionID", SqlDbType.VarChar);
                            parSessionID.Value = strSessionID;
                            comSession.Parameters.Add(parSessionID);
                            SqlDataAdapter daPurchases = new SqlDataAdapter(comSession);
                            daPurchases.Fill(dsPurchases);
                            return new OkObjectResult(dsPurchases.Tables[0]);
                        }
                    }
                }
                else
                {
                    return new OkObjectResult("{\"SessionID\":\"Endpoint Does Not Exist\"}");
                }
            } catch (Exception ex)
            {
                return new OkObjectResult(ex.Message.ToString());
            }

            return new OkObjectResult("RESPONSE HERE");
        }
    }
}
