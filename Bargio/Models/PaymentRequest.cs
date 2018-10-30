using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class PaymentRequest
    {
        public PaymentRequest() {
            DateDemande = DateTime.Now;
        }

        [Key]
        public string ID { get; set; } // PK
        [DataType(DataType.Currency)]
        public decimal Montant { get; set; }
        public DateTime DateDemande { get; set; }
    }
}
