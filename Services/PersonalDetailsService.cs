using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;

public class PersonalDetailsService : IPersonalDetailsService
{
    private readonly IConfiguration _config;

    public PersonalDetailsService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> SavePersonalDetails(PersonalDetails details)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_SaveOrUpdatePersonalDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", details.EmployeeId);
                cmd.Parameters.AddWithValue("@Name", details.Name);
                cmd.Parameters.AddWithValue("@DOB", details.DOB);
                cmd.Parameters.AddWithValue("@Gender", details.Gender);
                cmd.Parameters.AddWithValue("@Email", details.Email);
                cmd.Parameters.AddWithValue("@MaritalStatus", details.MaritalStatus);
                cmd.Parameters.AddWithValue("@Nationality", details.Nationality);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
        }
    }

    public async Task<PersonalDetails?> GetPersonalDetails(int employeeId)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_GetPersonalDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new PersonalDetails
                        {
                            EmployeeId = (int)reader["EmployeeId"],
                            Name = reader["Name"]?.ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Gender = reader["Gender"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            MaritalStatus = reader["MaritalStatus"]?.ToString(),
                            Nationality = reader["Nationality"]?.ToString()
                        };
                    }
                }
            }
        }

        return null; // not found
    }
}
