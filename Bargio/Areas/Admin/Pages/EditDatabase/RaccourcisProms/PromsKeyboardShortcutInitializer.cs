using System.Linq;
using System.Threading.Tasks;
using Bargio.Data;
using Bargio.Models;

namespace Bargio.Areas.Admin.Pages.EditDatabase.RaccourcisProms
{
    public class PromsKeyboardShortcutInitializer
    {
        public static async Task SeedData
            (ApplicationDbContext context) {
            var promsKeyboardShortcut = context.PromsKeyboardShortcut;
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F1"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F1",
                    TBK = "Autre",
                    Proms = ""
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F12"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F12",
                    TBK = "Li",
                    Proms = "218"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F11"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F11",
                    TBK = "Li",
                    Proms = "217"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F10"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F10",
                    TBK = "Li",
                    Proms = "216"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F9"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F9",
                    TBK = "Li",
                    Proms = "215"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F8"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F8",
                    TBK = "Li",
                    Proms = "214"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F7"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F7",
                    TBK = "Li",
                    Proms = "213"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F6"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F6",
                    TBK = "Li",
                    Proms = "212"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F5"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F5",
                    TBK = "Li",
                    Proms = "211"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F4"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F4",
                    TBK = "Li",
                    Proms = "210"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F3"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F3",
                    TBK = "Li",
                    Proms = "209"
                });
            if (!promsKeyboardShortcut.Any(o => o.Raccourci == "F2"))
                await promsKeyboardShortcut.AddAsync(new PromsKeyboardShortcut {
                    Raccourci = "F2",
                    TBK = "Li",
                    Proms = "208"
                });
            
            await context.SaveChangesAsync();
        }
    }
}
