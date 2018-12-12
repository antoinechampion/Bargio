﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.RaccourcisProms
{
    public class CreateModel : PageModel
    {
        private readonly Bargio.Data.ApplicationDbContext _context;

        public CreateModel(Bargio.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PromsKeyboardShortcut PromsKeyboardShortcut { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PromsKeyboardShortcut.Add(PromsKeyboardShortcut);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}