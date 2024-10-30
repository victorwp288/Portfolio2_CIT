using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAcessLayer.Users;

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
        var context = new ImdbContext();
        var service = new DataService(context);
        var items = service.GetUsers();
        Assert.Equal(2, items.Count);
        Assert.Equal("testuser1", items.First().Username);
        Assert.Equal("testuser1@example.com", items.First().Email);
        Assert.Equal("testuser2", items.Last().Username);
    }

    [Fact]
    public void User_GetUserById()
    {
        var context = new ImdbContext();
        var service = new DataService(context);
        var item = service.GetUserById(1);
        Assert.Equal("testuser1", item.Username);
        Assert.Equal("testuser1@example.com", item.Email);
    }
}
