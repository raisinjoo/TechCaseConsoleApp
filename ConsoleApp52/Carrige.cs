using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp52
{
    class Carrige
    {
        [Key]
        public int CarrigeId { get; set; }
        public int PositionInTrain { get; set; }
        public int CarNumber { get; set; }
        public string InvoiceNum { get; set; }
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public Operation Operation { get; set; }
        public int CargoId { get; set; }
        [ForeignKey("CargoId")]
        public Cargo Cargo { get; set; }
        public int FreightTotalWeightKg { get; set; }
    }
}
