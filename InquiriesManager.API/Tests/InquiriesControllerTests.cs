using Xunit;
using InquiriesManager.API.Controllers;
using InquiriesManager.API.Data;
using InquiriesManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

public class InquiriesControllerTests
{
    private InquiriesController GetControllerWithData(IEnumerable<Inquiry> inquiries = null)
    {
        var context = new TestDbContext();
        if (inquiries != null)
        {
            context.Inquiries.AddRange(inquiries);
            context.SaveChanges();
        }
        var repo = new InquiriesRepository(context);
        return new InquiriesController(repo);
    }

    [Fact]
    public async Task Get_ReturnsAllInquiries()
    {
        var inquiries = new List<Inquiry>
        {
            new Inquiry { Id = 1, Name = "A", Phone = "123", Email = "a@a.com", DepartmentId = 1, Description = "desc", CreatedAt = DateTime.Now },
            new Inquiry { Id = 2, Name = "B", Phone = "456", Email = "b@b.com", DepartmentId = 2, Description = "desc2", CreatedAt = DateTime.Now }
        };
        var controller = GetControllerWithData(inquiries);
        var result = await controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<Inquiry>>(okResult.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task Get_ById_ReturnsInquiry_WhenExists()
    {
        var inquiry = new Inquiry { Id = 1, Name = "A", Phone = "123", Email = "a@a.com", DepartmentId = 1, Description = "desc", CreatedAt = DateTime.Now };
        var controller = GetControllerWithData(new[] { inquiry });
        var result = await controller.Get(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<Inquiry>(okResult.Value);
        Assert.Equal(1, returned.Id);
    }

    [Fact]
    public async Task Get_ById_ReturnsNotFound_WhenNotExists()
    {
        var controller = GetControllerWithData();
        var result = await controller.Get(99);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Post_CreatesInquiry_ReturnsCreatedAtAction()
    {
        var controller = GetControllerWithData();
        var inquiry = new Inquiry { Name = "C", Phone = "789", Email = "c@c.com", DepartmentId = 3, Description = "desc3", CreatedAt = DateTime.Now };
        var result = await controller.Post(inquiry);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<Inquiry>(createdResult.Value);
        Assert.Equal("C", returned.Name);
    }

    [Fact]
    public async Task Put_UpdatesInquiry_ReturnsNoContent()
    {
        var inquiry = new Inquiry { Id = 1, Name = "D", Phone = "000", Email = "d@d.com", DepartmentId = 4, Description = "desc4", CreatedAt = DateTime.Now };
        var controller = GetControllerWithData(new[] { inquiry });
        inquiry.Name = "Updated";
        var result = await controller.Put(1, inquiry);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenIdMismatch()
    {
        var inquiry = new Inquiry { Id = 1, Name = "E", Phone = "111", Email = "e@e.com", DepartmentId = 5, Description = "desc5", CreatedAt = DateTime.Now };
        var controller = GetControllerWithData(new[] { inquiry });
        var result = await controller.Put(2, inquiry);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesInquiry_ReturnsNoContent()
    {
        var inquiry = new Inquiry { Id = 1, Name = "F", Phone = "222", Email = "f@f.com", DepartmentId = 6, Description = "desc6", CreatedAt = DateTime.Now };
        var controller = GetControllerWithData(new[] { inquiry });
        var result = await controller.Delete(1);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetMonthlyInquiriesReportAsync_ReturnsReportRows()
    {
        var controller = GetControllerWithData();
        var result = await controller.GetMonthlyInquiriesReportAsync(2024, 6);
        Assert.IsType<List<MonthlyInquiriesReportRow>>(result);
    }
}

// Dummy context for testing
public class TestDbContext : InquiriesDbContext
{
    public TestDbContext() : base(new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InquiriesDbContext>().Options) { }
}
