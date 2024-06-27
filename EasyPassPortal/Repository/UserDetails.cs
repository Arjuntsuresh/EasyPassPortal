using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Principal;
using EasyPassPortal.Models;
using System.Data;
using System.Web.ModelBinding;
using System.Data.Common;

namespace EasyPassPortal.Repository
{
   
    public class UserDetails
    {
        private SqlConnection UserDBConnection;
        private void UserConnection()
        {
            string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
            UserDBConnection = new SqlConnection(ConfigurationConnection);
        }

        //create user
        public bool AddAccountDetails(UserModel user)
        {
            UserConnection();
            SqlCommand sqlCommand = new SqlCommand("AddUserDetails", UserDBConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@Email", user.Email);
            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
            UserDBConnection.Open();
            int i = sqlCommand.ExecuteNonQuery();
            UserDBConnection.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifyUser(string email, string password)
        {
            SqlConnection UserDBConnection = null;
            try
            {
                string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
                UserDBConnection = new SqlConnection(ConfigurationConnection);
                UserDBConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetUserLoginDetail", UserDBConnection);
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


        //Create Passport
        public bool AddPassportDetails(UserPassportDetails userPassportDetails)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("AddPassportDetail", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@FullName", userPassportDetails.FullName);
                sqlCommand.Parameters.AddWithValue("@FatherName", userPassportDetails.FatherName);
                sqlCommand.Parameters.AddWithValue("@Gender", userPassportDetails.Gender);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", userPassportDetails.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Address", userPassportDetails.Address);
                sqlCommand.Parameters.AddWithValue("@Religion", userPassportDetails.Religion);
                sqlCommand.Parameters.AddWithValue("@State", userPassportDetails.State);
                sqlCommand.Parameters.AddWithValue("@Nationality", userPassportDetails.Nationality);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", userPassportDetails.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@Email", userPassportDetails.Email);
                UserDBConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
                UserDBConnection.Close();
                return result > 0;
            }catch
            {
                return false;
            }
        }

        public UserModel GetUserByEmail(string email)
        {
            UserConnection();
            SqlCommand sqlCommand = new SqlCommand("GetUserDetails", UserDBConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Email", email);

            UserDBConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            UserModel userModel = null;

            if (reader.Read())
            {
                userModel = new UserModel
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Email = Convert.ToString(reader["Email"]),
                    Password = Convert.ToString(reader["Password"])
                };
            }

            UserDBConnection.Close();
            return userModel;
        }


    }
}