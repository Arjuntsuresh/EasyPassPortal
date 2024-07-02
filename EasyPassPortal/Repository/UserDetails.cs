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
                SqlCommand checkCommand = new SqlCommand("CheckUserExist", UserDBConnection);
                checkCommand.CommandType = CommandType.StoredProcedure;
                checkCommand.Parameters.AddWithValue("@Email", user.Email);

                UserDBConnection.Open();
                int userExists = Convert.ToInt32(checkCommand.ExecuteScalar());
                UserDBConnection.Close();

                if (userExists == 0) 
                {
                    SqlCommand addCommand = new SqlCommand("AddUserDetails", UserDBConnection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.Parameters.AddWithValue("@Email", user.Email);
                    addCommand.Parameters.AddWithValue("@Password", user.Password);

                    UserDBConnection.Open();
                    int result = addCommand.ExecuteNonQuery();
                    UserDBConnection.Close();

                    return result > 0; 
                }

                return false;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
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
                SqlCommand sqlCommand = new SqlCommand("AddPassportDetail", UserDBConnection); // Ensure the stored procedure name matches
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@FullName", userPassportDetails.FullName);
                sqlCommand.Parameters.AddWithValue("@FatherName", userPassportDetails.FatherName);
                sqlCommand.Parameters.AddWithValue("@Gender", userPassportDetails.Gender);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", userPassportDetails.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Address", userPassportDetails.Address);
                sqlCommand.Parameters.AddWithValue("@Religion", userPassportDetails.Religion);
                sqlCommand.Parameters.AddWithValue("@State", userPassportDetails.State);
                sqlCommand.Parameters.AddWithValue("@District", userPassportDetails.District);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", userPassportDetails.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@Email", userPassportDetails.Email);
                sqlCommand.Parameters.AddWithValue("@AadarNumber", userPassportDetails.AadharNumber); // Ensure parameter names match
                sqlCommand.Parameters.AddWithValue("@PancardNumber", userPassportDetails.PancardNumber);
                sqlCommand.Parameters.AddWithValue("@Education", userPassportDetails.Education);
                sqlCommand.Parameters.AddWithValue("@Status", userPassportDetails.Status);
                UserDBConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
                UserDBConnection.Close();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
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
                    sqlCommand.Parameters.AddWithValue("@Email", user.Email);
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

        public UserPassportDetails GetUserPassportDetails(string email)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("GetUserPassportDetails", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Email", email);

                UserDBConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                UserPassportDetails userPassport = null;

                if (reader.Read())
                {
                    userPassport = new UserPassportDetails
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        FullName = Convert.ToString(reader["FullName"]),
                        FatherName = Convert.ToString(reader["FatherName"]),
                        Gender = Convert.ToString(reader["Gender"]),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Address = Convert.ToString(reader["Address"]),
                        Religion = Convert.ToString(reader["Religion"]),
                        State = Convert.ToString(reader["State"]),
                        District = Convert.ToString(reader["District"]),
                        PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                        Email = Convert.ToString(reader["Email"]),
                        AadharNumber = Convert.ToString(reader["AadarNumber"]),
                        PancardNumber = Convert.ToString(reader["PancardNumber"]),
                        Education = Convert.ToString(reader["Education"]),
                        Status = Convert.ToString(reader["Status"])
                    };
                }

                UserDBConnection.Close();
                return userPassport;
            }
            catch
            {
                return null;
            }
        }


    }
}