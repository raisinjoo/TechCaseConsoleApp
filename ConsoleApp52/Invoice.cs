using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp52
{
    class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceNum { get; set; }
    }
}
