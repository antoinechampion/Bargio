//          Bargio - PaymentRequest.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class PaymentRequest
    {
        public PaymentRequest() {
            DateDemande = DateTime.Now;
        }

        [Key] public string ID { get; set; } // PK

        public string UserName { get; set; }
        public decimal Montant { get; set; }
        public DateTime DateDemande { get; set; }
    }
}