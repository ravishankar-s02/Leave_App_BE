namespace LeaveAPI.Models
{
    public class LeaveStatusUpdateModel
    {
        public int LeaveId { get; set; }
        public string Status { get; set; }  // "APPROVE" or "REJECT"
    }
}
