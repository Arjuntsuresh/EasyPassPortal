using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using EasyPassPortal.Models;

namespace EasyPassPortal.Repository
{
    public class AdminDetails
    {
        private SqlConnection UserDBConnection;
        private void UserConnection()
        {
            string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
            UserDBConnection = new SqlConnection(ConfigurationConnection);
        }
        public bool VerifyAdmin(string email, string password)
        {
            SqlConnection UserDBConnection = null;
            try
            {
                string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
                UserDBConnection = new SqlConnection(ConfigurationConnection);
                UserDBConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetAdminDetails", UserDBConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Email", email);
                sqlCommand.Parameters.AddWithValue("@Password", password);

                // Execute the stored procedure and get the result
                int result = (int)sqlCommand.ExecuteScalar();
                // If result is greater than 0, user exists
                return result > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                // Ensure the connection is closed properly
                if (UserDBConnection != null && UserDBConnection.State == System.Data.ConnectionState.Open)
                {
                    UserDBConnection.Close();
                }
            }
        }

        //Get all passport applyed persons details
        public List<UserPassportDetails> GetAllDetails()
        {
            UserConnection();
            List<UserPassportDetails> userPassportDetails = new List<UserPassportDetails>();
            SqlCommand command = new SqlCommand("GetRegisteredPassportDetails", UserDBConnection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            UserDBConnection.Open();
            adapter.Fill(dataTable);
            UserDBConnection.Close();
            //binding the data
            foreach (DataRow dataRow in dataTable.Rows)
                userPassportDetails.Add(
                    new UserPassportDetails
                    {
                        Id = Convert.ToInt32(dataRow["id"]),
                        FullName = Convert.ToString(dataRow["FullName"]),
                        FatherName = Convert.ToString(dataRow["FatherName"]),
                        Gender = Convert.ToString(dataRow["Gender"]),
                        DateOfBirth = Convert.ToString(dataRow["DateOfBirth"]),
                        Address = Convert.ToString(dataRow["Address"]),
                        Religion = Convert.ToString(dataRow["Religion"]),
                        State = Convert.ToString(dataRow["State"]),
                        Nationality= Convert.ToString(dataRow["Nationality"]),
                        PhoneNumber = Convert.ToString(dataRow["PhoneNumber"]),
                        Email = Convert.ToString(dataRow["Email"])

                    }
                );
            return userPassportDetails;
        }
        //Get all user details
        public List<UserModel> GetAllUserDetails()
        {
            UserConnection();
            List<UserModel> userModels = new List<UserModel>();
            SqlCommand command = new SqlCommand("GetUserDetailsForAdmin", UserDBConnection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            UserDBConnection.Open();
            adapter.Fill(dataTable);
            UserDBConnection.Close();
            //binding the data
            foreach (DataRow dataRow in dataTable.Rows)
                userModels.Add(
                    new UserModel
                    {
                        UserID = Convert.ToInt32(dataRow["UserID"]),
                        Email = Convert.ToString(dataRow["Email"]),
                        Password = Convert.ToString(dataRow["password"]),

                    }
                );
            return userModels;
        }
    }

}