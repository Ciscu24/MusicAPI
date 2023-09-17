using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Common.Utilities;
using System.Diagnostics.CodeAnalysis;
using Common.Dtos.Entities;

namespace Models.Models
{
    [Table("Songs")]
    public class SongModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public SongQuality Quality { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [ForeignKey("Genre")]
        public long GenreId { get; set; }

        public virtual GenreModel Genre { get; set; }

        public virtual ICollection<ArtistSongModel> ArtistsSongs { get; set; }

        #region Métodos

        public SongDto ToDto(bool includes = false)
        {
            var song = InitializeDto();

            if (includes)
            {
                if (this.Genre != null)
                    song.Genre = this.Genre.ToDto();

                if (this.ArtistsSongs != null)
                    song.ArtistsSongs = this.ArtistsSongs.Select(s => s.ToDto());
            }

            return song;
        }

        private SongDto InitializeDto()
        {
            return new SongDto()
            {
                Id = this.Id,
                Title = this.Title,
                Duration = this.Duration,
                Quality = this.Quality,
                ReleaseDate = this.ReleaseDate,
                GenreId = this.GenreId
            };
        }

        #endregion
    }
}
