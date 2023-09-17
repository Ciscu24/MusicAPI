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
    [Table("Genres")]
    public class GenreModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<SongModel> Songs { get; set; }

        #region Métodos

        public GenreDto ToDto(bool includes = false)
        {
            var genre = InitializeDto();

            if (includes)
                if (this.Songs != null)
                    genre.Songs = this.Songs.Select(s => s.ToDto());

            return genre;
        }

        private GenreDto InitializeDto()
        {
            return new GenreDto()
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        #endregion
    }
}
