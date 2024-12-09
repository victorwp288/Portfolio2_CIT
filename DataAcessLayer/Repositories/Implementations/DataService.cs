using DataAcessLayer.Context;
using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories.Implementations
{
    public class DataService : IDataService
    {
        private readonly ImdbContext _context;

        public DataService(ImdbContext context)
        {
            _context = context;
        }

        public IList<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<UserBookmark> GetUserBookmerksByUserId(int id)
        {
            return _context.UserBookmarks.Where(x => x.UserId == id);
        }

        public bool FunctionLoginUser(string inputUsername, string inputPassword)
        {
            return CallFunctionReturnsBool($"SELECT login_user('{inputUsername}', '{inputPassword}')");

        }


        public int FunctionRegisterUser(string inputUsername, string inputEmail, string inputPassword)
        {
            //return VoidInt($"SELECT register_user('{inputUsername}', '{inputEmail}', '{inputPassword}')"); 
            bool result = VoidBool($"SELECT register_user('{inputUsername}', '{inputEmail}', '{inputPassword}')");
            if (result)
            {
                return _context.Users.Max(u => u.UserId);
            }
            else
            {
                return 0;
            }
        }

        public int FunctionRegisterUser(string inputUsername, string inputEmail, string inputPassword, string? inputRole)
        {
            bool result = VoidBool($"SELECT register_user('{inputUsername}', '{inputEmail}', '{inputPassword}', '{inputRole}')");
            if (result)
            {
                return _context.Users.Max(u => u.UserId);
            }
            else
            {
                return 0;
            }
        }

        public bool FunctionUpdateUserEmail(int inputUserId, string inputEmail)
        {
            return VoidBool($"SELECT update_user_email({inputUserId}, '{inputEmail}')");
        }

        public bool FunctionUpdateUserPassword(int inputUserId, string inputPassword)
        {
            return VoidBool($"SELECT update_user_password({inputUserId},  '{inputPassword}')");
        }
        public bool FunctionUpdateUserRole(int inputUserId, string inputRole)
        {
            return VoidBool($"SELECT update_user_role({inputUserId}, '{inputRole}')");
        }


        public bool DeleteUser(int id)
        {

            var user = _context.Users.Find(id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            return _context.SaveChanges() > 0;

        }
        public bool FunctionLogUserSearch(int inputUserId, string searchQuery)
        {
            return VoidBool($"SELECT log_user_search({inputUserId}, '{searchQuery}')");

        }

        public bool FunctionAddBookmark(int inputUserId, string inputMovieId, string note)
        {
            try
            {
                return VoidBool($"SELECT add_bookmark({inputUserId}, '{inputMovieId}', '{note}')");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"General Exception: {ex.Message}");
                return false;

            }
        }
        public bool FunctionManageBookmark(int inputUserId, string inputMovieId, string note)
        {
            return VoidBool($"SELECT manage_bookmark({inputUserId}, '{inputMovieId}', '{note}')");

        }
        public bool FunctionRateMovie(int inputUserId, string inputMovieId, int newRating)
        {
            return VoidBool($"SELECT rate_movie({inputUserId}, '{inputMovieId}', {newRating})");
        }

        public IList<TconstAndPrimaryTitle> FunctionExactMatchQuery(string w1Text, string w2Text, string w3Text)
        {
            return _context.TconstAndPrimaryTitles.FromSqlInterpolated($"SELECT * from exact_match_query('{w1Text}', '{w2Text}', '{w3Text}')").ToList();
        }

        public IList<TconstAndPrimaryTitle> FunctionOtherMoviesLikeThis(string pSearchName)
        {
            return _context.TconstAndPrimaryTitles.FromSqlInterpolated($"SELECT * from other_movies_like_this('{pSearchName}')").ToList();
        }

        public IList<TconstAndPrimaryTitle> FunctionSearchMovies(string pSearchText)
        {
            return _context.TconstAndPrimaryTitles.FromSqlInterpolated($"SELECT * from search_movies('{pSearchText}')").ToList();
        }

        public IList<TconstAndPrimaryTitle> FunctionStructuredSearch(string pTitle, string pPlot, string pActor)
        {
            return _context.TconstAndPrimaryTitles.FromSqlInterpolated($"SELECT * from structured_search('{pTitle}', '{pPlot}', '{pActor}')").ToList();
        }

        public IList<WordAndFrequency> FunctionPersonWords(string pName, int pLimit)
        {
            return _context.WordAndFrequencies.FromSqlInterpolated($"SELECT * from person_words({pName}, {pLimit})").ToList();
        }

        public IList<WordAndFrequency> FunctionWordToWordsQuery(int resultLimit, string[] keywords)
        {

            string sql = $"SELECT * from word_to_words_query({resultLimit}, {string.Join(",", keywords.Select(k => $"'{k}'"))})";


            return _context.WordAndFrequencies
                .FromSqlRaw(sql) // Usei FromSqlRaw instead of FromSqlInterpolated
                .ToList();
        }

        public IList<BestMatchQuery> FunctionBestMatchQuery(string w1Text, string w2Text, string w3Text)
        {
            return _context.BestMatchQueries.FromSqlInterpolated($"SELECT * from best_match_query({w1Text}, {w2Text}, {w3Text})").ToList();
        }

        public IList<GetMovieActorsByPopularity> FunctionGetMovieActorsByPopularity(string pTconst)
        {
            return _context.GetMovieActorsByPopularity.FromSqlInterpolated($"SELECT * from get_movie_actors_by_popularity({pTconst})").ToList();
        }

        public IList<SearchCoPlayer> FunctionSearchCoPlayer(string pSearchName)
        {
            return _context.SearchCoPlayers.FromSqlInterpolated($"SELECT * from search_co_players({pSearchName})").ToList();
        }

        public IList<SearchName> FunctionSearchName(string pSearchText)
        {
            return _context.SearchNames.FromSqlInterpolated($"SELECT * from search_names({pSearchText})").ToList();
        }

        public IList<NameBasic> GetNameBasics()
        {
            return _context.NameBasics.ToList();
        }

        public async Task<NameBasic> GetNameBasicByNconst(string nConst) {
            try 
            { 
                var result = await _context.NameBasics.Include(x => x.TitlePrincipals)
                     //.Include(x => x.PersonKnownTitles)
                     //.Include(x => x.PersonProfessions)
                     .Include(x => x.NameRatings)
                     .FirstOrDefaultAsync(x => x.Nconst == nConst);
                if (result != null)
                {
                    result.PersonKnownTitles = await GetPersonKnownTitlesByNconst(nConst);
                    result.PersonProfessions = await GetPersonProfessionsByNconst(nConst);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SQL: " + ex.Message);
                return null; // or throw the exception, depending on your error handling strategy.
            }
        }
        public async Task<IList<PersonKnownTitle>> GetPersonKnownTitlesByNconst(string nConst)
        {
            return await _context.PersonKnownTitles.Where(x => x.Nconst == nConst).ToListAsync();
        }

        public async Task<IList<PersonProfession>> GetPersonProfessionsByNconst(string nConst)
        {
            return await _context.PersonProfessions.Where(x => x.Nconst == nConst).ToListAsync();
        }

        public string CallFunctionReturnsString(string sql)
        {
            try
            {
                var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
                connection.Open();
                var command = new NpgsqlCommand(sql, connection);
                var result = command.ExecuteScalar();
                // Check for null result first
                if (result == null)
                {
                    connection.Close();
                    return null; // No result, likely means the SQL returned nothing
                }

                // Safely convert the result to boolean
                string returnValue = Convert.ToString(result);

                connection.Close();
                return returnValue;

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error executing SQL: " + ex.Message);
                return null;
            }
        }




        public bool CallFunctionReturnsBool(string sql)
        {
            try
            {
                var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
                connection.Open();
                var command = new NpgsqlCommand(sql, connection);
                var result = command.ExecuteScalar();
                // Check for null result first
                if (result == null)
                {
                    connection.Close();
                    return false; // No result, likely means the SQL returned nothing
                }

                // Safely convert the result to boolean
                bool returnValue = Convert.ToBoolean(result);

                connection.Close();
                return returnValue;

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error executing SQL: " + ex.Message);
                return false;
            }
        }

        public bool VoidBool(string sql)
        {
            try
            {
                var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
                connection.Open();
                var command = new NpgsqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error executing SQL: " + ex.Message);
                return false;
            }
        }
        public T CallFunctionWithOneReturn<T>(string sql)
        {
            using var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            connection.Open();
            using var command = new NpgsqlCommand(sql, connection);
            return (T)command.ExecuteScalar();

        }

        public TitleBasic GetTitleBasic(string id)
        {
            return _context.TitleBasics
                .Include(x => x.PersonKnownTitles)
                .Include(x => x.TitleRating)
                .Include(x => x.MovieGenres)
                .FirstOrDefault(x => x.Tconst == id);
        }


        public IList<TitleBasic> GetTitleBasics(int page, int pagesize)
        {
            return _context.TitleBasics
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToList();
        }

        public int GetNumberOfTitleBasics()
        {

            return _context.TitleBasics.Count();
        }
    }

}
