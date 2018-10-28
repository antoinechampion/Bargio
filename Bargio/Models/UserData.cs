using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class UserData
    {
        public UserData() {
            DateDerniereModif = DateTime.Now;
        }

        [Key]
        public string UserName { get; set; } // PK
        [DataType(DataType.Currency)]
        public decimal Solde { get; set; }
        public uint Nums { get; set; }
        public string TBK { get; set; }
        public uint Proms { get; set; }
        public bool HorsFoys { get; set; }
        public string Surnom { get; set; }
        public DateTime DateDerniereModif { get; set; }
    }
}
