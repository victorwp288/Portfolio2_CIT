using Microsoft.EntityFrameworkCore;
using DataAcessLayer;

namespace Portfolio2_Tests
{
    public class TestDbContext : ImdbContext
    {
        public TestDbContext(DbContextOptions<ImdbContext> options)
            : base(options)
        {
        }

        // Override OnConfiguring to prevent the base class from using the actual database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Do not call the base method to prevent connection to the actual database
        }
    }
}
