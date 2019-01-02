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

        /* Paramètre zifoy'ss de la babasse */
        public bool MiseHorsBabasseAutoActivee { get; set; }
        public decimal MiseHorsBabasseSeuil { get; set; }
        // Instantanée ou périodique
        public bool MiseHorsBabasseInstantanee { get; set; }
        // Quotidienne ou hebdomadaire
        public bool MiseHorsBabasseQuotidienne { get; set; }
        public string MiseHorsBabasseQuotidienneHeure { get; set; }
        public string MiseHorsBabasseHebdomadaireJours { get; set; }
        public string MiseHorsBabasseHebdomadaireHeure { get; set; }
        public string MotDePasseZifoys { get; set; }

        public string MotDesZifoys { get; set; }
        public string Actualites { get; set; }
        public bool Snow { get; set; }
    }
}
