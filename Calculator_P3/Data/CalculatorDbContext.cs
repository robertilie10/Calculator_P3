using Calculator_P3.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator_P3.Data
{
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Calculator> Calculators { get; set; }
    }
}
