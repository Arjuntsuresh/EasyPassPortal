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
using System.Web.Mvc;

namespace EasyPassPortal.Repository
{
   
    public class UserDetails
    {
        private SqlConnection UserDBConnection;

        public object ViewBag { get; private set; }

        /// <summary>
        /// DB connection 
        /// </summary>
        private void UserConnection()
        {
            string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
            UserDBConnection = new SqlConnection(ConfigurationConnection);
        }

       /// <summary>
       /// User adding the details and registering to the web site.
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public bool AddAccountDetails(UserModel user)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("AddUserDetails", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                sqlCommand.Parameters.AddWithValue("@Password", user.Password);
                UserDBConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
                UserDBConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Verifing user login page.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
            catch 
            {
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


        /// <summary>
        /// Creating the passport for user.
        /// </summary>
        /// <param name="userPassportDetails"></param>
        /// <returns></returns>
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
        /// <summary>
        /// User profile getting from db using session.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserModel GetUserByEmail(string email)
        {
            try
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
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Edit password of user from user side.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public void EditPassword (UserModel user)
        {
            try
            {
                UserConnection();
                using (SqlCommand sqlCommand = new SqlCommand("UpdateUserPassword", UserDBConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserID", user.UserID);
                    sqlCommand.Parameters.AddWithValue("@Password", user.Password);

                    UserDBConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Log.Error(ex);
            }
            finally
            {
                if (UserDBConnection.State == ConnectionState.Open)
                {
                    UserDBConnection.Close();
                }
            }
        }


    }
}