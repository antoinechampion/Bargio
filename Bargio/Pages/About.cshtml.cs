﻿//          Bargio - About.cshtml.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bargio.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet() {
            Message = "Your application description page.";
        }
    }
}