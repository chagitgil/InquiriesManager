-- Stored Procedure: MonthlyInquiriesReport
-- Returns: Department, CurrentMonthCount, PreviousMonthCount, LastYearSameMonthCount

CREATE PROCEDURE MonthlyInquiriesReport
    @ReportYear INT,
    @ReportMonth INT
AS
BEGIN
    SELECT 
        Department,
        COUNT(CASE WHEN YEAR(CreatedAt) = @ReportYear AND MONTH(CreatedAt) = @ReportMonth THEN 1 END) AS CurrentMonthCount,
        COUNT(CASE WHEN YEAR(CreatedAt) = @ReportYear AND MONTH(CreatedAt) = @ReportMonth - 1 THEN 1 END) AS PreviousMonthCount,
        COUNT(CASE WHEN YEAR(CreatedAt) = @ReportYear - 1 AND MONTH(CreatedAt) = @ReportMonth THEN 1 END) AS LastYearSameMonthCount
    FROM Inquiries
    GROUP BY Department
END
GO


