using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Station
    {
        [Key]
        public int StationId { get; set; }
        public string StationName { get; set; }
    }
}
