using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp52
{
    class Train
    {
        [Key]
        public int TrainId { get; set; }
        public int TrainNumber { get; set; }
        public int TrainIndex { get; set; }
        public string TrainIndexCombined { get; set; }
        public int? FromStationId { get; set; }
        [ForeignKey("FromStationId")]
        public virtual Station FromStation { get; set; }
        public int? ToStationId { get; set; }
        [ForeignKey("ToStationId")]
        public virtual Station ToStation { get; set; }

        public int CarrigeId { get; set; }
        [ForeignKey("CarrigeId")]
        public Carrige Carrige { get; set; }


    }
}
