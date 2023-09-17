using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.Entities
{
    public class SongDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public SongQuality Quality { get; set; }

        public DateTime ReleaseDate { get; set; }

        public long GenreId { get; set; }

        public virtual GenreDto Genre { get; set; }

        public virtual IEnumerable<ArtistSongDto> ArtistsSongs { get; set; }
    }
}
