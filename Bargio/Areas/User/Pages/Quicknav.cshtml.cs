using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Areas.User.Pages
{
    [Authorize]
    public class QuicknavModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}