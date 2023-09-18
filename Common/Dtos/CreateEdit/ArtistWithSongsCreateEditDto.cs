using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.CreateEdit
{
    public class ArtistWithSongsCreateEditDto
    {
        public string Name { get; set; }

        public List<SongCreateEditDto> Songs { get; set; }
    }
}
