using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bargio.Areas.Identity
{
    public class IdentityUserDefaultPwd : IdentityUser<string>
    {

        private string _nums;
        public override string UserName {
            get => this._nums;
            set {
                _nums = value.ToLower();
                SecurityStamp = Guid.NewGuid().ToString();
            }   
        }

        public static readonly string DefaultPassword = "default";
    }
}
