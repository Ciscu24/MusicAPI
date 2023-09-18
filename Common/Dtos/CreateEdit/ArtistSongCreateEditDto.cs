using Common.Dtos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.CreateEdit
{
    public class ArtistSongCreateEditDto
    {
        public long ArtistId { get; set; }

        public long SongId { get; set; }
    }
}
