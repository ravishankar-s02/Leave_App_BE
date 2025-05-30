using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;

public class ContactDetailsService : IContactDetailsService
{
    private readonly IConfiguration _config;

    public ContactDetailsService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> SaveContactDetails(ContactDetails details)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_SaveOrUpdateContactDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", details.EmployeeId);
                cmd.Parameters.AddWithValue("@PhoneNumber", details.PhoneNumber);
                cmd.Parameters.AddWithValue("@AlternateNumber", details.AlternateNumber);
                cmd.Parameters.AddWithValue("@Email", details.Email);
                cmd.Parameters.AddWithValue("@StreetAddress", details.StreetAddress);
                cmd.Parameters.AddWithValue("@City", details.City);
                cmd.Parameters.AddWithValue("@State", details.State);
                cmd.Parameters.AddWithValue("@ZipCode", details.ZipCode);
                cmd.Parameters.AddWithValue("@Country", details.Country);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
        }
    }

    public async Task<ContactDetails?> GetContactDetails(int employeeId)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_GetContactDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ContactDetails
                        {
                            EmployeeId = (int)reader["EmployeeId"],
                            PhoneNumber = reader["PhoneNumber"]?.ToString(),
                            AlternateNumber = reader["AlternateNumber"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            StreetAddress = reader["StreetAddress"]?.ToString(),
                            City = reader["City"]?.ToString(),
                            State = reader["State"]?.ToString(),
                            ZipCode = reader["ZipCode"]?.ToString(),
                            Country = reader["Country"]?.ToString()
                        };
                    }
                }
            }
        }

        return null; // not found
    }
}
