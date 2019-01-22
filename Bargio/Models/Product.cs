//          Bargio - Product.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

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