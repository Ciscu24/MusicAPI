using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.Generic
{
    public class CreateEditRemoveResponseDto
    {
        public object Id { get; set; }

        public List<string> Errors { get; set; }

        public CreateEditRemoveResponseDto() 
        {
            Id = null;
            Errors = new List<string>();
        }
    }
}
