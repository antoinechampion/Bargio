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
        public string ErrorTitle { get; set; }
        
        public void OnGet(int statusCode = 404) {
            ErrorTitle = "Erreur " + statusCode;
            switch (statusCode) {
            case 403:
                ErrorMessage = "Cette tentative d'accès non autorisé a été rhopsée.";
                break;
            case 503:
                ErrorMessage = "On fait une maintenance, " 
                    + "reviens plus tard";
                break;
            default:
                ErrorMessage = "Une erreur " + statusCode + " a été rhopsée.";
                break;
            }
            
        }
    }
}
