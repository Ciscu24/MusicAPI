using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dtos.Entities;

namespace Models.Models
{
    [Table("Artists")]
    public class ArtistModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ArtistSongModel> ArtistsSongs { get; set; }

        #region Métodos

        public ArtistDto ToDto(bool includes = false)
        {
            var artist = InitializeDto();

            if (includes)
                if (this.ArtistsSongs != null)
                    artist.ArtistsSongs = this.ArtistsSongs.Select(s => s.ToDto());

            return artist;
        }

        private ArtistDto InitializeDto()
        {
            return new ArtistDto()
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        #endregion
    }
}
