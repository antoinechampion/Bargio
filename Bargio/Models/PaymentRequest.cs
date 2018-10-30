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
        public string UserName { get; set; }
        public decimal Montant { get; set; }
        public DateTime DateDemande { get; set; }
    }
}
