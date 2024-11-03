using DataAcessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Portfolio2_Tests
{
    public abstract class BaseRepositoryTests : IDisposable
    {
        protected readonly TestDbContext _context;
        protected readonly string _connectionString;

        protected BaseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _connectionString = "host=localhost;db=imdb;uid=postgres;pwd=Hejmed12!";
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 