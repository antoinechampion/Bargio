//          Bargio - TransactionHistory.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class TransactionHistory
    {
        public TransactionHistory() {
            Date = DateTime.Now;
        }

        [Key] public string ID { get; set; } // PK

        public string UserName { get; set; }
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)] public decimal Montant { get; set; }

        // = null dans le cas d'un rechargement
        public string IdProduits { get; set; }

        // Ex: 2 x Jupiler, Rechargement en ligne...
        public string Commentaire { get; set; }
    }
}