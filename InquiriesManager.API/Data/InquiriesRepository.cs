using InquiriesManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InquiriesManager.API.Data
{
    public class InquiriesRepository
    {
        private readonly InquiriesDbContext _context;

        public InquiriesRepository(InquiriesDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inquiry>> GetAllAsync() => await _context.Inquiries.ToListAsync();

        public async Task<Inquiry?> GetByIdAsync(int id) => await _context.Inquiries.FindAsync(id);

        public async Task AddAsync(Inquiry inquiry)
        {
            _context.Inquiries.Add(inquiry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inquiry inquiry)
        {
            _context.Inquiries.Update(inquiry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry != null)
            {
                _context.Inquiries.Remove(inquiry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MonthlyInquiriesReportRow>> GetMonthlyInquiriesReportAsync(int reportYear, int reportMonth)
        {
            // נקודת ייחוס
            var current = new DateTime(reportYear, reportMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            var prev = current.AddMonths(-1);
            var lastYr = current.AddYears(-1);

            // גבולות טווח לכל חודש (כולל התחלה, עד לפני סוף)
            var curStart = current;
            var curEnd = current.AddMonths(1);

            var prevStart = prev;
            var prevEnd = prev.AddMonths(1);

            var lastStart = lastYr;
            var lastEnd = lastYr.AddMonths(1);

            // סינון מקדים לשלושה החודשים הרלוונטיים בלבד (יעיל יותר)
            return await _context.Inquiries
                .Where(i =>
                    (i.CreatedAt >= prevStart && i.CreatedAt < curEnd) ||
                    (i.CreatedAt >= lastStart && i.CreatedAt < lastEnd))
                .GroupBy(i => i.DepartmentId)
                .Select(g => new MonthlyInquiriesReportRow
                {
                    DepartmentId = g.Key,
                    CurrentMonthCount = g.Count(i => i.CreatedAt >= curStart && i.CreatedAt < curEnd),
                    PreviousMonthCount = g.Count(i => i.CreatedAt >= prevStart && i.CreatedAt < prevEnd),
                    LastYearSameMonthCount = g.Count(i => i.CreatedAt >= lastStart && i.CreatedAt < lastEnd)
                })
                .OrderBy(r => r.DepartmentId)
                .ToListAsync();
        }
    }
}

