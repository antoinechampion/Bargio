//          Bargio - Error.cshtml.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

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