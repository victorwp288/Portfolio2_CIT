using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAcessLayer.Entities.Users;
using static System.Collections.Specialized.BitVector32;
using DataAcessLayer.Context;
using DataAcessLayer.Repositories.Implementations;
using DataAcessLayer.Entities.Functions;
using Microsoft.EntityFrameworkCore;

namespace Portfolio2_Tests;

public class DataServiceTests
{
    private ImdbContext _context;
    private DataService _service;

    // This method runs once before all tests
    public DataServiceTests()
    {
        _context = new ImdbContext(); // Initialize context here
        _service = new DataService(_context); // Initialize service using the context
    }


    /* Users */
    [Fact]
    public void User_Object_HasIdUsernameEmailPassDateRolt()
    {
        var usre = new User();
        Assert.Equal(0, usre.UserId);
        Assert.Null(usre.Username);
        Assert.Null(usre.Email);
        Assert.Null(usre.PasswordHash);
        Assert.Null(usre.Role);
    }

    [Fact]
    public void RegisterUsre_ValidData_RegistereUserAndReturnsNewObject()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        Assert.True(id > 0);
        // cleanup
        _service.DeleteUser(id);
    }

    [Fact]
    public void User_GetUsers()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var items = _service.GetUsers();
        //Assert.Equal(1, items.Count);
        Assert.Equal("testuser1", items.Last().Username);
        Assert.Equal("testuser1@example.com", items.Last().Email);
        _service.DeleteUser(id);
    }

    [Fact]
    public void User_GetUserById()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var item = _service.GetUserById(id);
        Assert.Equal("testuser1", item.Username);
        Assert.Equal("testuser1@example.com", item.Email);
        _service.DeleteUser(id);
    }
    [Fact]
    public void User_DeleteUser()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var result = _service.DeleteUser(id);
        Assert.True(result);
        var user = _service.GetUserById(id);
        Assert.Null(user);
    }

    [Fact]
    public void Function_User_UpdateUserEmail()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var result = _service.FunctionUpdateUserEmail(id, "user1@example.com");
        Assert.True(result);
        var user = _service.GetUserById(id);
        Assert.Equal("testuser1", user.Username);
        Assert.Equal("user1@example.com", user.Email);
        _service.DeleteUser(id);
    }
    [Fact]
    public void Function_User_UpdateUserPassword()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var result = _service.FunctionUpdateUserPassword(id, "1234");
        Assert.True(result);
        result = _service.FunctionLoginUser("testuser1", "1234");
        Assert.True(result);
        _service.DeleteUser(id);
    }
    [Fact]
    public void Function_User_UpdateUserRole()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var result = _service.FunctionUpdateUserRole(id, "admin");
        Assert.True(result);
        var user = _service.GetUserById(id);
        Assert.Equal("testuser1", user.Username);
        Assert.Equal("testuser1@example.com", user.Email);
        Assert.Equal("admin", user.Role);
        _service.DeleteUser(id);
    }

    [Fact]
    public void Function_User_LoginUser()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        var result = _service.FunctionLoginUser("testuser1", "password123");
        Assert.True(result);
        _service.DeleteUser(id);
    }

    

    [Fact]
    public void Function_Rate_Movie()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        string tConst = "tt9603212";
        var result = _service.FunctionRateMovie(id, tConst, 7);
        Assert.True(result);
        var userRating = _service.GetUserRatingReviewWithUserIdAndTconst(id, tConst);
        Assert.Equal(7, userRating.Rating);
        _service.DeleteUserBookmark(id, tConst);
        _service.DeleteUser(id);
    }

    [Fact]
    public void Function_Manage_Bookmark()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        string tConst = "tt9603212";
        var resultAddBmark = _service.FunctionAddBookmark(id, tConst, "A Movie of Tom");
        var resultMngBmark = _service.FunctionManageBookmark(id, tConst, "It is a Good Movie");
        Assert.True(resultMngBmark);
        var bMarkMB = _service.GetUserBookmarkWithUserIdAndTconst(id, tConst);
        Assert.Equal("It is a Good Movie", bMarkMB.Note);
        _service.DeleteUserBookmark(id, tConst);
        _service.DeleteUser(id);
    }

    [Fact]
    public void Function_Add_Bookmark()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        string tConst = "tt9603212";
        var resultAddBmark = _service.FunctionAddBookmark(id, tConst , "A Movie of Tom");
        Assert.True(resultAddBmark);
        var bMarkAB = _service.GetUserBookmarkWithUserIdAndTconst(id,tConst);
        Assert.Equal("A Movie of Tom", bMarkAB.Note);
        
        _service.DeleteUserBookmark(id, tConst);
        _service.DeleteUser(id); 
    }
    
    [Fact]
    public void Function_Other_Movies_Like_This()
    {

        var items = _service.FunctionOtherMoviesLikeThis("dangerous");
        Assert.Equal(100216, items.Count);
        Assert.Equal("tt8030814", items.First().Tconst);
        Assert.Equal("The Blech Effect", items.First().PrimaryTitle);
        Assert.Equal("tt8030814", items.Last().Tconst);
        Assert.Equal("The Big Bang", items.Last().PrimaryTitle);
    }


    [Fact]
    public void Function_Structured_Search()
    {

        var items = _service.FunctionStructuredSearch("Mission", "action","Tom Cruise");
        Assert.Equal(1, items.Count);
        Assert.Equal("tt9603212 ", items.First().Tconst);
        Assert.Equal("Mission: Impossible - Dead Reckoning Part One", items.First().PrimaryTitle);
    }

    [Fact]
    public void Function_Person_Words()
    {

        var items = _service.FunctionPersonWords("Tom Cruise", 5);
        Assert.Equal(5, items.Count);
        Assert.Equal("tom", items.First().Word);
        Assert.Equal(62, items.First().Frequency);
        Assert.Equal("pritan", items.Last().Word);
        Assert.Equal(26, items.Last().Frequency);
    }

    [Fact]
    public void Function_Word_To_Words_Query()
    {
        string[] st = { "adventure", "action" };
        var items = _service.FunctionWordToWordsQuery(10, st);
        Assert.Equal(10, items.Count);
        Assert.Equal("adventure", items.First().Word);
        Assert.Equal(29, items.First().Frequency);
        Assert.Equal("mark", items.Last().Word);
        Assert.Equal(6, items.Last().Frequency);
    }

    [Fact]
    public void Function_Best_Match_Query()
    {
        var items = _service.FunctionBestMatchQuery("Adventure", "action", "Thriller");
        Assert.Equal(3271, items.Count);
        Assert.Equal("Brad Pitt: Breaking Hollywood", items.First().PrimaryTitle);
        Assert.Equal(3, items.First().Rank);
        Assert.Equal("The J-K Conspiracy", items.Last().PrimaryTitle);
        Assert.Equal(1, items.Last().Rank);
    }

    [Fact]
    public void Function_Get_Movie_Actors_By_Popularity()
    {
        var items = _service.FunctionGetMovieActorsByPopularity("tt21880152");
        Assert.Equal(5, items.Count);
        Assert.Equal("Katherine Kubler", items.First().PrimaryName);
        Assert.Equal(8.80, items.First().WeightedRating);
        Assert.Equal("Henry Cavill", items.Last().PrimaryName);
        Assert.Equal(7.83, items.Last().WeightedRating);
    }

    [Fact]
    public void Function_Search_Co_Player()
    {
        var items = _service.FunctionSearchCoPlayer("Tom Cruise");
        Assert.Equal(321, items.Count);
        Assert.Equal("Pritan Ambroase", items.First().PrimaryName);
        Assert.Equal(34, items.First().Frequency);
        Assert.Equal("Jahmika Mitchell", items.Last().PrimaryName);
        Assert.Equal(1, items.Last().Frequency);
    }

    [Fact]
    public void Function_Search_Name()
    {
        var items = _service.FunctionSearchName("Tom");
        Assert.Equal(2282, items.Count);
        Assert.Equal("Tom Lowell", items.First().PrimaryName);
        Assert.Equal("The Twilight Zone", items.First().PrimaryTitle);
        Assert.Equal("Tom Allom", items.Last().PrimaryName);
        Assert.Equal("Judas Priest: Battle Cry", items.Last().PrimaryTitle);
    }
    /*
    [Fact]
    public void Function_Log_User_Search()
    {
        var id = _service.FunctionRegisterUser("testuser1", "testuser1@example.com", "password123");
        string searchQuery = "adventure";
        var result = _service.FunctionLogUserSearch(id, searchQuery);
        Assert.True(result);
        _service.DeleteUsersLastSearchHistoryByUserId(id);
        _service.DeleteUser(id);
    }
    [Fact]
    public void Function_Exact_Match_Query()
    {
        var items = _service.FunctionExactMatchQuery("adventure", "action", "dangerous");
        string firstTconst = "tt0362619";
        string lastTconst = "tt12415660";
        Assert.Equal(2, items.Count);
        Assert.Equal(firstTconst, items.First().Tconst);
        Assert.Equal("Fans and Freaks: The Culture of Comics and Conventions", items.First().PrimaryTitle);
        Assert.Equal(lastTconst, items.Last().Tconst);
        Assert.Equal("Bazyl", items.Last().PrimaryTitle);
    }
 */

}
