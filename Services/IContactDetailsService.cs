using LeaveAPI.Models;
using System.Threading.Tasks;

public interface IContactDetailsService
{
    Task<ContactDetails?> GetContactDetails(int employeeId);  // ✅ fixed
    Task<bool> SaveContactDetails(ContactDetails details);
}
