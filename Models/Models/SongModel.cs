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

        public virtual ICollection<ArtistModel> Artists { get; set; }
    }
}
