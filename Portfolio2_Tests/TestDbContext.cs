using Microsoft.EntityFrameworkCore;
using DataAcessLayer.Context;
using Microsoft.Extensions.Configuration;

namespace Portfolio2_Tests
{
    public class TestDbContext : ImdbContext
    {
        public TestDbContext(DbContextOptions<ImdbContext> options)
            : base(options, CreateTestConfiguration())
        {
        }

        private static IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:ImdbDatabase", "Host=localhost;Database=imdb_test;Username=test;Password=test"}
                })
                .Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Do not call base to prevent actual database connection
        }
    }
}
