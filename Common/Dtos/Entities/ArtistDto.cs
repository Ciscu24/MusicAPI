using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.Entities
{
    public class ArtistDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<ArtistSongDto> ArtistsSongs { get; set; }
    }
}
