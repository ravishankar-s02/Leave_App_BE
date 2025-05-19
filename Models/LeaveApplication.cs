namespace LeaveAPI.Models
{
    public class LeaveApplication
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public required string Status {get; set;}
        public string TypeName {get; set;}
}

}
