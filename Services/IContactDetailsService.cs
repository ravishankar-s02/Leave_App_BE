using LeaveAPI.Models;
using System.Threading.Tasks;

public interface IContactDetailsService
{
    Task<ContactDetails?> GetContactDetails(int employeeId);
    Task<bool> SaveContactDetails(ContactDetails details);
}
