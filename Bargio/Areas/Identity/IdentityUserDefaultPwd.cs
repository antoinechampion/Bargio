using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Identity
{
    public class IdentityUserDefaultPwd : IdentityUser<string>
    {

        public string Nums { get; set; }

        public override string UserName {
            get => Nums.ToLower();
            set => Nums = value;
        }

        public static readonly string DefaultPassword = "default";
    }
}
