using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataService;

namespace Portfolio2_Tests;

public class DataServiceTests
{
    /* Users */
    [Fact]
    public void User_Object_HasIdUsernameEmailPassDateRolt()
    {
        var category = new User();
        Assert.Equal(0, category.UserId);
        Assert.Null(category.Username);
        Assert.Null(category.Email);
        Assert.Null(category.PasswordHash);
        Assert.Null(category.Role);
    }
    [Fact]
    public void User_GetUsers()
    {
        var service = new DataService.DataService();
        var items = service.GetUsers();
        Assert.Equal(2, items.Count);
        Assert.Equal("testuser1", items.First().Username);
        Assert.Equal("testuser1@example.com", items.First().Email);
        Assert.Equal("testuser2", items.Last().Username);
    }
    

}
