using EasyPassPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EasyPassPortal.Repository
{
    public class PoliceDetail
    {
        private SqlConnection UserDBConnection;
        private void UserConnection()
        {
            string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
            UserDBConnection = new SqlConnection(ConfigurationConnection);
        }

        public bool VerifyPolice(string email, string password)
        {
            SqlConnection UserDBConnection = null;
            try
            {
                string ConfigurationConnection = ConfigurationManager.ConnectionStrings["UserData"].ToString();
                UserDBConnection = new SqlConnection(ConfigurationConnection);
                UserDBConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("GetPoliceDetails", UserDBConnection);
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

        public List<UserPassportDetails> GetAllDetails()
        {
            try
            {
                UserConnection();
                List<UserPassportDetails> userPassportDetails = new List<UserPassportDetails>();
                SqlCommand command = new SqlCommand("GetRegisteredPassportDetailsForPolice", UserDBConnection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                UserDBConnection.Open();
                adapter.Fill(dataTable);
                UserDBConnection.Close();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    userPassportDetails.Add(new UserPassportDetails
                    {
                        Id = Convert.ToInt32(dataRow["id"]),
                        FullName = Convert.ToString(dataRow["FullName"]),
                        FatherName = Convert.ToString(dataRow["FatherName"]),
                        Gender = Convert.ToString(dataRow["Gender"]),
                        DateOfBirth = Convert.ToString(dataRow["DateOfBirth"]),
                        Address = Convert.ToString(dataRow["Address"]),
                        Religion = Convert.ToString(dataRow["Religion"]),
                        State = Convert.ToString(dataRow["State"]),
                        District = Convert.ToString(dataRow["District"]),
                        PhoneNumber = Convert.ToString(dataRow["PhoneNumber"]),
                        Email = Convert.ToString(dataRow["Email"]),
                        AadharNumber = Convert.ToString(dataRow["AadarNumber"]),  // Corrected spelling
                        PancardNumber = Convert.ToString(dataRow["PancardNumber"]),
                        Education = Convert.ToString(dataRow["Education"]),
                        Status = Convert.ToString(dataRow["Status"]),
                        Image = dataRow["Image"] != DBNull.Value ? (byte[])dataRow["Image"] : null,
                        MothersName = Convert.ToString(dataRow["MothersName"]),
                        AadharPhoto = dataRow["AadharPhoto"] != DBNull.Value ? (byte[])dataRow["AadharPhoto"] : null,
                        IdProof = dataRow["IdProof"] != DBNull.Value ? (byte[])dataRow["IdProof"] : null,
                    });
                }

                return userPassportDetails;
            }
            catch
            {
                return null;
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
                        DateOfBirth = Convert.ToString(reader["DateOfBirth"]),
                        Address = Convert.ToString(reader["Address"]),
                        Religion = Convert.ToString(reader["Religion"]),
                        State = Convert.ToString(reader["State"]),
                        District = Convert.ToString(reader["District"]),
                        PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                        Email = Convert.ToString(reader["Email"]),
                        AadharNumber = Convert.ToString(reader["AadarNumber"]),
                        PancardNumber = Convert.ToString(reader["PancardNumber"]),
                        Education = Convert.ToString(reader["Education"]),
                        Status = Convert.ToString(reader["Status"]),
                        Image = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null,
                        AadharPhoto = reader["AadharPhoto"] != DBNull.Value ? (byte[])reader["AadharPhoto"] : null,
                        IdProof = reader["IdProof"] != DBNull.Value ? (byte[])reader["IdProof"] : null,
                        MothersName = Convert.ToString(reader["MothersName"])
                    };
                }

                reader.Close();
                UserDBConnection.Close();
                return userPassport;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool EditAccountDetails(UserPassportDetails userPassportDetails)
        {
            try
            {
                UserConnection();
                SqlCommand sqlCommand = new SqlCommand("UpdateUserPassportDetails", UserDBConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@id", userPassportDetails.Id);
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