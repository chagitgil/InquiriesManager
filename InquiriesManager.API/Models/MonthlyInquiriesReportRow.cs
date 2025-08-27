namespace InquiriesManager.API.Models
{
    public class MonthlyInquiriesReportRow
    {
        public int DepartmentId { get; set; }
        public int CurrentMonthCount { get; set; }
        public int PreviousMonthCount { get; set; }
        public int LastYearSameMonthCount { get; set; }
    }
}
