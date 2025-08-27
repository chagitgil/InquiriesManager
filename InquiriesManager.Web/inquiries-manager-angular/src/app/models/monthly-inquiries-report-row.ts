//מודל דוח פניות חודשי
export interface MonthlyInquiriesReportRow {
    departmentId: number;
    currentMonthCount: number;
    previousMonthCount: number;
    lastYearSameMonthCount: number;
}