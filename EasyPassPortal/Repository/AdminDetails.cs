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
                            DateOfBirth = Convert.ToDateTime(dataRow["DateOfBirth"]),
                            Address = Convert.ToString(dataRow["Address"]),
                            Religion = Convert.ToString(dataRow["Religion"]),
                            State = Convert.ToString(dataRow["State"]),
                            District = Convert.ToString(dataRow["District"]),
                            PhoneNumber = Convert.ToString(dataRow["PhoneNumber"]),
                            Email = Convert.ToString(dataRow["Email"]),
                            AadharNumber = Convert.ToString(dataRow["AadarNumber"]),
                            PancardNumber = Convert.ToString(dataRow["PancardNumber"]),
                            Education = Convert.ToString(dataRow["Education"]),
                            Status = Convert.ToString(dataRow["Status"])


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
        /// <summary>
        /// Add new admin 
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Edit password for admin.
        /// </summary>
        /// <param name="adminModel"></param>
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
        /// <summary>
        /// Delete user from admin side.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete passport detail from admin side.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeletePassportDetailFromAdmin(int id)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("DeletePassportDetailAccount", UserDBConnection);
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
        /// <summary>
        /// Total users count.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int TotalCountOfUser()
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("GetTotalUserCount", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                UserDBConnection.Open();
                var result = sqlCommand.ExecuteScalar();
                UserDBConnection.Close();

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while getting the total count of users.", ex);
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
        /// Total passpost request count.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int TotalPassportRequestCountOfUser()
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("GetTotalPassportCount", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                UserDBConnection.Open();
                var result = sqlCommand.ExecuteScalar();
                UserDBConnection.Close();

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while getting the total count of users.", ex);
            }
            finally
            {
                if (UserDBConnection.State == ConnectionState.Open)
                {
                    UserDBConnection.Close();
                }
            }
        }

        public bool EditAccountDetails(UserPassportDetails userPassportDetails)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("UpdateUserPassportDetails", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@id",userPassportDetails.Id);
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
                sqlCommand.Parameters.AddWithValue("@AadharNumber", userPassportDetails.AadharNumber);
                sqlCommand.Parameters.AddWithValue("@PancardNumber", userPassportDetails.PancardNumber);
                sqlCommand.Parameters.AddWithValue("@Education", userPassportDetails.Education);
                sqlCommand.Parameters.AddWithValue("@Status", userPassportDetails.Status);
                UserDBConnection.Open();
                int result = sqlCommand.ExecuteNonQuery();
                UserDBConnection.Close();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }



    }

}