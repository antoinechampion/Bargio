using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bargio.Models
{
    public class SystemParameters
    {
        [Key]
        public string IpServeur { get; set; }
        public DateTime DerniereConnexionBabasse { get; set; }
        public bool BucquagesBloques { get; set; }
        public bool LydiaBloque { get; set; }
        public bool Maintenance { get; set; }
    }
}
