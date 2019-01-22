//          Bargio - IdentityUserDefaultPwd.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Identity
{
    public class IdentityUserDefaultPwd : IdentityUser<string>
    {
        public static readonly string DefaultPassword = "default";

        public string Nums { get; set; }

        public override string UserName {
            get => Nums.ToLower();
            set => Nums = value;
        }
    }
}