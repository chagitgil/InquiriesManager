CREATE PROCEDURE dbo.MonthlyInquiriesReport
    @ReportYear  int,
    @ReportMonth int
AS
BEGIN
    SET NOCOUNT ON;

    -- צמצום התאריכים עליהם תתבצע השליפה
    DECLARE @curStart  date = DATEFROMPARTS(@ReportYear, @ReportMonth, 1);
    DECLARE @curEnd    date = DATEADD(MONTH, 1, @curStart);

    DECLARE @prevStart date = DATEADD(MONTH, -1, @curStart);
    DECLARE @prevEnd   date = @curStart;

    DECLARE @lastStart date = DATEADD(YEAR, -1, @curStart);
    DECLARE @lastEnd   date = DATEADD(MONTH, 1, @lastStart);

    ;WITH R AS (
        SELECT Department, 'CUR' AS B
        FROM dbo.Inquiries
        WHERE CreatedAt >= @curStart AND CreatedAt < @curEnd
        UNION ALL
        SELECT Department, 'PREV'
        FROM dbo.Inquiries
        WHERE CreatedAt >= @prevStart AND CreatedAt < @prevEnd
        UNION ALL
        SELECT Department, 'LAST'
        FROM dbo.Inquiries
        WHERE CreatedAt >= @lastStart AND CreatedAt < @lastEnd
    )
    SELECT
        Department,
        SUM(CASE WHEN B='CUR'  THEN 1 ELSE 0 END) AS CurrentMonthCount,
        SUM(CASE WHEN B='PREV' THEN 1 ELSE 0 END) AS PreviousMonthCount,
        SUM(CASE WHEN B='LAST' THEN 1 ELSE 0 END) AS LastYearSameMonthCount
    FROM R
    GROUP BY Department;
END
GO

-- שיפור ביצועים
/*
הוספת אינדקסים על CreatedAt ו Department
בטבלאות ענקיות הוספת Partition


*/

