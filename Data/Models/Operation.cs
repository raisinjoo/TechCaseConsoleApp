using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Operation
    {
        [Key]
        public int OperationId { get; set; }
        public DateTime WhenLastOperation { get; set; }
        public string LastOperationName { get; set; }
        public int LastStationId { get; set; }
        [ForeignKey("LastStationId")]
        public Station Station { get; set; }
    }
}
