using logincatalogue.Models;
using Microsoft.EntityFrameworkCore;

namespace logincatalogue.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext>option): base(option)
        {
            
      
        }
        public DbSet<account> Account { get; set; }
    }
    
}
