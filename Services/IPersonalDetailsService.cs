using LeaveAPI.Models;
using System.Threading.Tasks;

public interface IPersonalDetailsService
{
    Task<PersonalDetails?> GetPersonalDetails(int employeeId);  // ✅ fixed
    Task<bool> SavePersonalDetails(PersonalDetails details);
}
