using DataAcessLayer.Entities.Movies;
public interface IMovieSearchRepository
{
    Task<IEnumerable<TitleBasic>> SearchMoviesAsync(string query);
}
