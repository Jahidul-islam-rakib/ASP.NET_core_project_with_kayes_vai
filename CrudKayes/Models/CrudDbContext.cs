
namespace CrudKayes.Models
{
	public class CrudDbContext : DbContext
	{
        public CrudDbContext(DbContextOptions<CrudDbContext>options): base(options)
        {
            
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Person> Person { get; set; }
	}
}
