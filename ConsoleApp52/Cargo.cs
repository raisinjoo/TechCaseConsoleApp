using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp52
{
    class Cargo
    {
        [Key]
        public int CargoId { get; set; }
        public string FreightEtsngName { get; set; }
    }
}
