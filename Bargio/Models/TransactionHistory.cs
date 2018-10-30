using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class TransactionHistory
    {
        public TransactionHistory() {
            Date = DateTime.Now;
        }

        [Key]
        public string ID { get; set; } // PK
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public decimal Montant { get; set; }
        // = null dans le cas d'un rechargement
        public uint? IdProduit { get; set; }
        // Ex: 2 x Jupiler, Rechargement en ligne...
        public string Commentaire { get; set; }
    }
}
