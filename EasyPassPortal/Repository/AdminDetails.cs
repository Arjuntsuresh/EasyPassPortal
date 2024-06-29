﻿using System;
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
        /// <summary>
        /// connection string db.
        /// </summary>
        private void UserConnection()
        {
            string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
            UserDBConnection = new SqlConnection(ConfigurationConnection);
        }
        /// <summary>
        /// Verify admin login.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// Get all passsport applyed persons details
        /// </summary>
        /// <returns></returns>
        public List<UserPassportDetails> GetAllDetails()
        {
            try
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
                            Nationality = Convert.ToString(dataRow["Nationality"]),
                            PhoneNumber = Convert.ToString(dataRow["PhoneNumber"]),
                            Email = Convert.ToString(dataRow["Email"])

                        }
                    );
                return userPassportDetails;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Get all registered user details for admin.
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUserDetails()
        {
            try
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
            catch
            {
            return null;
            }
        }

        public bool AddNewAdmin(AdminModel adminModel)
        {
            try
            {
                UserConnection();
                SqlCommand command = new SqlCommand("AddNewAdminDetails", UserDBConnection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", adminModel.Email);
                command.Parameters.AddWithValue("@Password", adminModel.Password);
                UserDBConnection.Open();
                command.ExecuteNonQuery();
                UserDBConnection.Close();
                return true;
            }catch
            {
                return false;
            }
        }

        public void EditPassword(AdminModel adminModel)
        {
            try
            {
                UserConnection();
                using (SqlCommand sqlCommand = new SqlCommand("UpdateAdminPassword", UserDBConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", adminModel.Id);
                    sqlCommand.Parameters.AddWithValue("@Password",adminModel.Password);

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
        /// <summary>
        /// The function is for getting the data of admin.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public AdminModel GetAdminByEmail(string email)
        {
            try
            {
                // Initialize the database connection
                UserConnection();

                // Ensure the database connection is properly initialized
                if (UserDBConnection == null)
                {
                    throw new InvalidOperationException("Database connection is not initialized.");
                }

                // Ensure the email parameter is not null or empty
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email parameter cannot be null or empty.", nameof(email));
                }

                using (SqlCommand sqlCommand = new SqlCommand("GetAdminDetailsByEmail", UserDBConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Email", email);

                    UserDBConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        AdminModel adminModel = null;

                        if (reader.Read())
                        {
                            adminModel = new AdminModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Email = Convert.ToString(reader["Email"]),
                                Password = Convert.ToString(reader["Password"])
                            };
                        }

                        return adminModel;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log SQL exceptions here
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                return null;
            }
            catch (InvalidOperationException invOpEx)
            {
                // Log invalid operation exceptions here
                Console.WriteLine($"Invalid Operation: {invOpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log any other exceptions here
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
            finally
            {
                // Ensure the connection is closed
                if (UserDBConnection != null && UserDBConnection.State == ConnectionState.Open)
                {
                    UserDBConnection.Close();
                }
            }
        }

        public bool DeleteUserFromAdmin(int id)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("DeleteUserAccount", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@id", id);
                UserDBConnection.Open();
                sqlCommand.ExecuteNonQuery();
                UserDBConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }



    }

}