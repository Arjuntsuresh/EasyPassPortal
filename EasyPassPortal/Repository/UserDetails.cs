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
                int result = (int)sqlCommand.ExecuteScalar();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if necessary
                return false;
            }
            finally
            {
                if (UserDBConnection != null && UserDBConnection.State == System.Data.ConnectionState.Open)
                {
                    UserDBConnection.Close();
                }
            }
        }

        //Create Passport
        public bool AddPassportDetails(UserPassportDetails userPassportDetails)
        {
            UserConnection();
            SqlCommand sqlCommand = new SqlCommand("AddPassportDetail", UserDBConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@FirstName", userPassportDetails.FullName);
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

    }
}