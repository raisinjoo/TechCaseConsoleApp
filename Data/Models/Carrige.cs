using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Carrige
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
