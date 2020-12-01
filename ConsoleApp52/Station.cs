using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp52
{
    class Station
    {
        [Key]
        public int StationId { get; set; }
        public string StationName { get; set; }
    }
}
