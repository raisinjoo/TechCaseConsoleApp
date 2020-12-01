using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Cargo
    {
        [Key]
        public int CargoId { get; set; }
        public string FreightEtsngName { get; set; }
    }
}
