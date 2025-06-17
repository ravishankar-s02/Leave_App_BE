using LeaveAPI.Models;
using System.Threading.Tasks;

public interface IJobService
{
    Task<Job?> GetJob(int employeeId);  // ✅ fixed
    Task<bool> SaveJob(Job details);
}
