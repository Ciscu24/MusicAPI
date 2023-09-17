using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.Entities
{
    public class ArtistSongDto
    {
        public long Id { get; set; }

        public long ArtistId { get; set; }

        public ArtistDto Artist { get; set; }

        public long SongId { get; set; }

        public SongDto Song { get; set; }
    }
}
