using LeaveAPI.Models;
using System.Threading.Tasks;

public interface IJobService
{
    Task<Job?> GetJob(int employeeId);
    Task<bool> SaveJob(Job details);
}
