namespace LeaveAPI.Models
{
    public class ApplyLeaves
    {
        public int EmployeeId { get; set; }
        public string? TypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
}

}
