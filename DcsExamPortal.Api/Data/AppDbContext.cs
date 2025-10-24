using DcsExamPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DcsExamPortal.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
