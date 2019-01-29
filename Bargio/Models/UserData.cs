//          Bargio - UserData.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class UserData
    {
        public UserData() {
            DateDerniereModif = DateTime.Now;
            ModeArchi = false;
        }

        [Key] public string UserName { get; set; } // PK

        [DataType(DataType.Currency)] public decimal Solde { get; set; }

        public string Nums { get; set; }
        public string TBK { get; set; }
        public string Proms { get; set; }
        public bool HorsFoys { get; set; }
        public string Surnom { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public DateTime DateDerniereModif { get; set; }

        public bool FoysApiHasPassword { get; set; }

        // To be hashed with Blowfish Crypt, rev 2B
        public string FoysApiPasswordHash { get; set; }
        public string FoysApiPasswordSalt { get; set; }
        public bool ModeArchi { get; set; }
        public bool CompteVerrouille { get; set; }
    }
}