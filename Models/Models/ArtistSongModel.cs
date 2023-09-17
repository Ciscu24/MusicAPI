using Common.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    [Table("ArtistsSongs")]
    public class ArtistSongModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [ForeignKey("Artist")]
        public long ArtistId { get; set; }

        public ArtistModel Artist { get; set; }

        [ForeignKey("Song")]
        public long SongId { get; set; }

        public SongModel Song { get; set; }

        #region Métodos

        public ArtistSongDto ToDto(bool includes = false)
        {
            var artistSong = InitializeDto();

            if (includes)
            {
                if (this.Artist != null)
                    artistSong.Artist = this.Artist.ToDto();

                if (this.Song != null)
                    artistSong.Song = this.Song.ToDto();
            }

            return artistSong;
        }

        private ArtistSongDto InitializeDto()
        {
            return new ArtistSongDto()
            {
                Id = this.Id,
                ArtistId = this.ArtistId,
                SongId = this.SongId
            };
        }

        #endregion
    }
}
