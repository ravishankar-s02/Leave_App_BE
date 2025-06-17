using LeaveAPI.Models;
using System.Threading.Tasks;

public interface ISalaryService
{
    Task<Salary?> GetSalary(int employeeId); 
    Task<bool> SaveSalary(Salary details);
}
