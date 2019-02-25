//          Bargio - SystemParameters.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class SystemParameters
    {
        [Key] public string Id { get; set; }

        [DisplayName("Dernière connexion à la babasse")]
        public DateTime DerniereConnexionBabasse { get; set; }
        
        [DisplayName("Bloquer les rechargements en ligne")]
        public bool LydiaBloque { get; set; }

        [DisplayName("Taux de commission variable lydia (%)")]
        [Range(0, 100, ErrorMessage = "On te demande un pourcentage trou de zizi")]
        public decimal CommissionLydiaVariable { get; set; }

        [DisplayName("Taux de commission fixe lydia (€)")]
        public decimal CommissionLydiaFixe { get; set; }

        [DisplayName("Minimum de rechargement en ligne (€)")]
        public decimal MinimumRechargementLydia { get; set; }

        [DisplayName("Mode maintenance  -- /!\\ Cette page deviendra inaccessible")]
        public bool Maintenance { get; set; }

        /* Paramètre zifoy'ss de la babasse */
        [DisplayName("Activer la mise hors babasse automatique")]
        public bool MiseHorsBabasseAutoActivee { get; set; }

        [DisplayName("Seuil de mise hors babasse automatique")]
        public decimal MiseHorsBabasseSeuil { get; set; }

        [DisplayName("Mise hors babasse auto instantanée (plutôt que périodique)")]
        // Instantanée ou périodique
        public bool MiseHorsBabasseInstantanee { get; set; }

        [DisplayName("Si périodique, mise hors babasse quotidienne (plutôt qu'hebdomadaire)")]
        // Quotidienne ou hebdomadaire
        public bool MiseHorsBabasseQuotidienne { get; set; }

        [DisplayName("Heure pour la mise hors babasse quotidienne si activée")]
        public string MiseHorsBabasseQuotidienneHeure { get; set; }

        [DisplayName("Jour(s) pour la mise hors babasse hebdomadaire si activée")]
        public string MiseHorsBabasseHebdomadaireJours { get; set; }

        [DisplayName("Heure pour chaque jour de la mise hors babasse hebdomadaire si activée")]
        public string MiseHorsBabasseHebdomadaireHeure { get; set; }

        [DisplayName("Mot de passe zifoys")] public string MotDePasseZifoys { get; set; }


        [DisplayName("Mot des Zifoys")] public string MotDesZifoys { get; set; }

        [DisplayName("Actualités")] public string Actualites { get; set; }

        [DisplayName("Oh, il neige !")] public bool Snow { get; set; }
    }
}