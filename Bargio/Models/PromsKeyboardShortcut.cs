using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bargio.Models
{
    public class PromsKeyboardShortcut
    {
        [Key]
        public string Raccourci { get; set; }
        public string TBK { get; set; }
        public string Proms { get; set; }
    }
}
