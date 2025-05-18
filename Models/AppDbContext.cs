using Microsoft.EntityFrameworkCore;

namespace IntegracionGemini.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<RespuestaIA> RespuestasIA { get; set; }
    }
}