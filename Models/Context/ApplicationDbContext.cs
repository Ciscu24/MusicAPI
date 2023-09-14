﻿using Common.Utilities;
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
            // Relación N:M de Artistas y Canciones
            modelBuilder.Entity<ArtistModel>()
                .HasMany(a => a.Songs)
                .WithMany(s => s.Artists)
                .UsingEntity(e => e.ToTable("ArtistsSongs"));
        }

        #region Métodos Privados

        private void GenerateDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GenreModel>().HasData(GenerateGenres());
            modelBuilder.Entity<ArtistModel>().HasData(GenerateArtists());
            modelBuilder.Entity<SongModel>().HasData(GenerateSongs());
            //modelBuilder.Entity<Dictionary<string, object>>().HasData(GenerateArtistsSongs());
        }

        private static GenreModel[] GenerateGenres()
        {
            return new[] {
                new GenreModel { Id = 1, Name = "Pop" },
                new GenreModel { Id = 2, Name = "Hip-hop" }
            };
        }

        private static ArtistModel[] GenerateArtists()
        {
            return new[] {
                new ArtistModel { Id = 1, Name = "Post Malone" },
                new ArtistModel{ Id = 2, Name = "Area 21" }
            };
        }

        private static SongModel[] GenerateSongs()
        {
            return new[] 
            {
                new SongModel { Id = 1, Title = "Mourning", Duration = 134, GenreId = 1, Quality = SongQuality.Excellent, ReleaseDate = new DateTime(2023, 5, 15) },
                new SongModel { Id = 2, Title = "Sunflower", Duration = 158, GenreId = 2, Quality = SongQuality.Good, ReleaseDate = new DateTime(2018, 1, 26) },
                new SongModel { Id = 3, Title = "Spaceships", Duration = 147, GenreId = 1, Quality = SongQuality.Bad, ReleaseDate = new DateTime(2016, 2, 8) },
                new SongModel { Id = 4, Title = "HELP", Duration = 212, GenreId = 1, Quality = SongQuality.Fair, ReleaseDate = new DateTime(2019, 7, 1) }
            };

        }

        private static Dictionary<string, object>[] GenerateArtistsSongs()
        {
            return new[]
            {
                new Dictionary<string, object> { { "ArtistId", 1 }, { "SongId", 1 } },
                new Dictionary<string, object> { { "ArtistId", 1 }, { "SongId", 2 } },
                new Dictionary<string, object> { { "ArtistId", 2 }, { "SongId", 3 } },
                new Dictionary<string, object> { { "ArtistId", 1 }, { "SongId", 4 } },
                new Dictionary<string, object> { { "ArtistId", 2 }, { "SongId", 4 } }
            };
        }

        #endregion
    }
}
