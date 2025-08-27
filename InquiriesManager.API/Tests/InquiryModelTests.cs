using Xunit;
using InquiriesManager.API.Models;
using System;

public class InquiryModelTests
{
    [Fact]
    public void Inquiry_CanBeCreated_WithValidProperties()
    {
        var now = DateTime.Now;
        var inquiry = new Inquiry
        {
            Id = 1,
            Name = "Test",
            Phone = "123456789",
            Email = "test@test.com",
            DepartmentId = 2,
            Description = "desc",
            CreatedAt = now
        };
        Assert.Equal(1, inquiry.Id);
        Assert.Equal("Test", inquiry.Name);
        Assert.Equal("123456789", inquiry.Phone);
        Assert.Equal("test@test.com", inquiry.Email);
        Assert.Equal(2, inquiry.DepartmentId);
        Assert.Equal("desc", inquiry.Description);
        Assert.Equal(now, inquiry.CreatedAt);
    }
}
