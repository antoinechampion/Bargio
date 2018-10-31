using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public string ErrorMessage { get; set; }


        public void OnGet(int errorCode = 404)
        {
            ErrorMessage = "Une erreur " + errorCode + " a été rhopsée.";
        }
    }
}
