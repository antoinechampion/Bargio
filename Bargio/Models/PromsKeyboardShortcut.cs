//          Bargio - PromsKeyboardShortcut.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.ComponentModel.DataAnnotations;

namespace Bargio.Models
{
    public class PromsKeyboardShortcut
    {
        [Key] public string ID { get; set; }

        public string Raccourci { get; set; }
        public string TBK { get; set; }
        public string Proms { get; set; }
    }
}