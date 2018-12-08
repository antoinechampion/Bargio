using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bargio.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public uint CompteurConsoMois { get; set; }
        public uint CompteurConsoTotal { get; set; }
        public string RaccourciClavier { get; set; }
    }
}
