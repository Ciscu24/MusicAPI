using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Models.Context
{
    public class ApplicationDbContext: DbContext
    {
        #region DbSets

        public DbSet<ArtistModel> ArtistsDb { get; set; }

        public DbSet<GenreModel> GenresDb { get; set; }

        public DbSet<SongModel> SongsDb { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistModel>()
                .HasMany(a => a.Songs)
                .WithMany(s => s.Artists)
                .UsingEntity(e => e.ToTable("ArtistsSongs"));
        }
    }
}
