using Xunit;
using InquiriesManager.API.Data;
using InquiriesManager.API.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class InquiriesRepositoryTests
{
    private InquiriesRepository GetRepoWithData(IEnumerable<Inquiry> inquiries = null)
    {
        var context = new TestDbContext();
        if (inquiries != null)
        {
            context.Inquiries.AddRange(inquiries);
            context.SaveChanges();
        }
        return new InquiriesRepository(context);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllInquiries()
    {
        var inquiries = new List<Inquiry>
        {
            new Inquiry { Id = 1, Name = "A", Phone = "123", Email = "a@a.com", DepartmentId = 1, Description = "desc", CreatedAt = DateTime.Now },
            new Inquiry { Id = 2, Name = "B", Phone = "456", Email = "b@b.com", DepartmentId = 2, Description = "desc2", CreatedAt = DateTime.Now }
        };
        var repo = GetRepoWithData(inquiries);
        var result = await repo.GetAllAsync();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsInquiry_WhenExists()
    {
        var inquiry = new Inquiry { Id = 1, Name = "A", Phone = "123", Email = "a@a.com", DepartmentId = 1, Description = "desc", CreatedAt = DateTime.Now };
        var repo = GetRepoWithData(new[] { inquiry });
        var result = await repo.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var repo = GetRepoWithData();
        var result = await repo.GetByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsInquiry()
    {
        var repo = GetRepoWithData();
        var inquiry = new Inquiry { Name = "C", Phone = "789", Email = "c@c.com", DepartmentId = 3, Description = "desc3", CreatedAt = DateTime.Now };
        await repo.AddAsync(inquiry);
        var all = await repo.GetAllAsync();
        Assert.Single(all);
        Assert.Equal("C", all.First().Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesInquiry()
    {
        var inquiry = new Inquiry { Id = 1, Name = "D", Phone = "000", Email = "d@d.com", DepartmentId = 4, Description = "desc4", CreatedAt = DateTime.Now };
        var repo = GetRepoWithData(new[] { inquiry });
        inquiry.Name = "Updated";
        await repo.UpdateAsync(inquiry);
        var updated = await repo.GetByIdAsync(1);
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesInquiry()
    {
        var inquiry = new Inquiry { Id = 1, Name = "F", Phone = "222", Email = "f@f.com", DepartmentId = 6, Description = "desc6", CreatedAt = DateTime.Now };
        var repo = GetRepoWithData(new[] { inquiry });
        await repo.DeleteAsync(1);
        var result = await repo.GetByIdAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetMonthlyInquiriesReportAsync_ReturnsReportRows()
    {
        var inquiries = new List<Inquiry>
        {
            new Inquiry { Id = 1, Name = "A", Phone = "123", Email = "a@a.com", DepartmentId = 1, Description = "desc", CreatedAt = new DateTime(2024, 6, 1) },
            new Inquiry { Id = 2, Name = "B", Phone = "456", Email = "b@b.com", DepartmentId = 1, Description = "desc2", CreatedAt = new DateTime(2024, 5, 1) },
            new Inquiry { Id = 3, Name = "C", Phone = "789", Email = "c@c.com", DepartmentId = 2, Description = "desc3", CreatedAt = new DateTime(2023, 6, 1) }
        };
        var repo = GetRepoWithData(inquiries);
        var report = await repo.GetMonthlyInquiriesReportAsync(2024, 6);
        Assert.NotNull(report);
        Assert.All(report, r => Assert.True(r.CurrentMonthCount >= 0));
    }
}

// Dummy context for testing
public class TestDbContext : InquiriesDbContext
{
    public TestDbContext() : base(new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InquiriesDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options) { }

    protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
    {
        // Ensure the in-memory provider is always used for tests
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        }
    }
}
