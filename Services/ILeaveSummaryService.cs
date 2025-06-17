using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Services
{
    public interface ILeaveSummaryService
    {
        List<LeaveSummaryModel> GetLeaveSummaryByEmployee(int employeeId);
        void UploadLeaveBalance(LeaveSummaryModel model);

    }
}