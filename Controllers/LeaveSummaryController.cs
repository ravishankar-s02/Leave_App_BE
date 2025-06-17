using Microsoft.AspNetCore.Mvc;
using LeaveAPI.Models;
using LeaveAPI.Services;

namespace LeaveAPI.Controllers
{
    [Route("api/leave-summary")]
    [ApiController]
    public class LeaveSummaryController : ControllerBase
    {
        private readonly ILeaveSummaryService _leaveSummaryService;

        public LeaveSummaryController(ILeaveSummaryService leaveSummaryService)
        {
            _leaveSummaryService = leaveSummaryService;
        }

        [HttpGet("{employeeId}")]
        public ActionResult<List<LeaveSummaryModel>> GetLeaveSummary(int employeeId)
        {
            var result = _leaveSummaryService.GetLeaveSummaryByEmployee(employeeId);
            return Ok(result);
        }

        [HttpPost("upload-balance")]
        public IActionResult UploadLeaveBalance([FromBody] LeaveSummaryModel model)
        {
            _leaveSummaryService.UploadLeaveBalance(model);
            return Ok(new { message = "Leave balance uploaded successfully" });
        }

    }
}