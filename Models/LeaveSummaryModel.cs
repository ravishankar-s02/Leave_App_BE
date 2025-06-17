namespace LeaveAPI.Models
{
    public class LeaveSummaryModel
    {
        public int EmployeeID { get; set; }
        public int Year { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveRemaining { get; set; }

        // Optional for other components
        public decimal LeaveScheduled { get; set; }
        public decimal LeaveTaken { get; set; }
    }


}