using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using LeaveAPI.Models;

public class SalaryService : ISalaryService
{
    private readonly IConfiguration _config;

    public SalaryService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> SaveSalary(Salary details)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_SaveOrUpdateSalary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", details.EmployeeId);
                cmd.Parameters.AddWithValue("@PayGrade", details.PayGrade);
                cmd.Parameters.AddWithValue("@Currency", details.Currency);
                cmd.Parameters.AddWithValue("@BasicSalary", details.BasicSalary);
                cmd.Parameters.AddWithValue("@PayFrequency", details.PayFrequency);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
        }
    }

    public async Task<Salary?> GetSalary(int employeeId)
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("sp_GetSalary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Salary
                        {
                            EmployeeId = (int)reader["EmployeeId"],
                            PayGrade = reader["PayGrade"]?.ToString(),
                            Currency = reader["Currency"]?.ToString(),
                            BasicSalary = reader["BasicSalary"]?.ToString(),
                            PayFrequency = reader["PayFrequency"]?.ToString()
                        };
                    }
                }
            }
        }

        return null; 
    }
}
