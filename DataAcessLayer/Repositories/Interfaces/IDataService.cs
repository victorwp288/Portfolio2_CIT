using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;


namespace DataAcessLayer;

public interface IDataService
{
    IList<User> GetUsers();

    User GetUserById(int id);

    Task<User> GetUserByUserNameAsync(string userName);

    public TitleBasic GetTitleBasic(string id);

    IList<TitleBasic> GetTitleBasics(int page, int pagesize);

    int GetNumberOfTitleBasics();

    bool FunctionLoginUser(string inputUsername, string inputPassword);

    int FunctionRegisterUser(string inputUsername, string inputEmail, string inputPassword, string? inputRole);

    int FunctionRegisterUser(string inputUsername, string inputEmail, string inputPassword);

    bool DeleteUser(int id);

    bool FunctionUpdateUserEmail(int inputUserId, string inputEmail);

    bool FunctionUpdateUserPassword(int inputUserId, string inputPassword);

    bool FunctionUpdateUserRole(int inputUserId, string inputRole);

    bool FunctionLogUserSearch(int inputUserId, string searchQuery);

    bool FunctionAddBookmark(int inputUserId, string inputMovieId, string note);

    bool FunctionManageBookmark(int inputUserId, string inputMovieId, string note);

    bool FunctionRateMovie(int inputUserId, string inputMovieId, int newRating);

    IList<TconstAndPrimaryTitle> FunctionExactMatchQuery(string w1Text, string w2Text, string w3Text);

    IList<TconstAndPrimaryTitle> FunctionOtherMoviesLikeThis(string pSearchName);

    IList<TconstAndPrimaryTitle> FunctionSearchMovies(string pSearchText);

    IList<TconstAndPrimaryTitle> FunctionStructuredSearch(string pTitle, string pPlot, string pActor);

    IList<WordAndFrequency> FunctionPersonWords(string pName, int pLimit);

    IList<WordAndFrequency> FunctionWordToWordsQuery(int resultLimit, string[] keywords);

    IList<BestMatchQuery> FunctionBestMatchQuery(string w1Text, string w2Text, string w3Text);

    IList<GetMovieActorsByPopularity> FunctionGetMovieActorsByPopularity(string pTconst);

    IList<SearchCoPlayer> FunctionSearchCoPlayer(string pSearchName);

    IList<SearchName> FunctionSearchName(string pSearchText);

    IList<NameBasic> GetNameBasics();

   
    IEnumerable<UserBookmark> GetUserBookmerksByUserId(int id);

    Task<NameBasic> GetNameBasicByNconst(string nConst);
    
    Task<IList<PersonKnownTitle>> GetPersonKnownTitlesByNconst(string nConst);

    Task<IList<PersonProfession>> GetPersonProfessionsByNconst(string nConst);

    Task<User> GetUserByEmailAsync(string email);
}
