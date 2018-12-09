using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bargio.Api
{
    [Route("api/[controller]")]
    public class ImportDbController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUserDefaultPwd> _userManager;

        public ImportDbController(ApplicationDbContext context,
            UserManager<IdentityUserDefaultPwd> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Extrait de fichier XML
        //<tbUser>
        //    <numero_utilisateur>2193</numero_utilisateur>
        //    <user_numss_Reel>49</user_numss_Reel>
        //    <user_numss_Babasse>49</user_numss_Babasse>
        //    <user_promss>cl215</user_promss>
        //    <user_bucque>Varzaks</user_bucque>
        //    <user_solde>-0.7</user_solde>
        //    <user_En_Babasse>0</user_En_Babasse>
        //    <user_compteur>0</user_compteur>
        //    <user_montant>0</user_montant>
        //    <user_bucque>snooopy</user_bucque>
        //    <user_Nom>fo</user_Nom>
        //    <user_Prénom>veto</user_Prénom>
        //    <user_Blairal>06xxxxxxxx</user_Blairal>
        //</tbUser>
        private UserData TryParseUserFromXElement(XElement o, ref string failedList)
        {
            try {
                return new UserData
                {
                    UserName = (string)o.Element("user_numss_Reel")
                               + (string)o.Element("user_promss"),
                    Solde = (decimal)o.Element("user_solde"),
                    Nums = (string)o.Element("user_numss_Reel"),
                    TBK = ((string)o.Element("user_promss")).Substring(0, 2),
                    Proms = ((string)o.Element("user_promss")).Substring(2),
                    HorsFoys = false,
                    Surnom = (string)o.Element("user_bucque") ?? "",
                    Nom = (string)o.Element("user_Nom") ?? "",
                    Prenom = (string)o.Element("user_Prénom") ?? "",
                    Telephone = (string)o.Element("user_Blairal") ?? "",
                    FoysApiHasPassword = false
                };
            }
            catch (Exception)
            {
                failedList += " " + (string) o.Element("user_numss_Reel") + (string) o.Element("user_promss");
                return null;
            }
        }

        private async Task<string> SaveContext() {
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return
                    "Impossible de mettre à jour la BDD: elle est actuellement utilisée.\n" +
                    "Veuillez bloquer tout accès au site quand vous réalisez cette opération.";
            }
            catch (DbUpdateException e) {
                return "Une erreur est survenue lors de la validation des changements:\n" + e;
            }

            return "";
        }
        
        // POST api/<controller>
        [HttpPost]
        public async Task<string> Post(List<IFormFile> xml_file, bool append_to_db)
        {
            var errorMessage = "";
            var result = string.Empty;
            try
            {
                using (var reader = new StreamReader(xml_file[0].OpenReadStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return "Echec de l'upload du fichier\n" + e;
            }

            XDocument xml;
            try
            {
                xml = XDocument.Parse(result);
            }
            catch (Exception e)
            {
                return "Fichier XML invalide\n" + e;
            }

            
            if (!append_to_db)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("delete from UserData");
                }
                catch (Exception e)
                {
                    errorMessage += "Impossible de vider la DB UserData actuelle\n" + e;
                }

                foreach (var user in _userManager.Users.ToList())
                {
                    var identityResult = await _userManager.DeleteAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        errorMessage += "Impossible de supprimer l'utilisateur "
                                        + user.UserName + "\n-->" + string.Join(",",
                                            identityResult.Errors.Select(o => o.ToString()));
                    }
                }
                var eraseContextError = await SaveContext();
                if (!string.IsNullOrEmpty(eraseContextError))
                    return eraseContextError;
            }

            if (!string.IsNullOrEmpty(errorMessage))
                return "\n\n" + errorMessage;

            string failedList = "";
            string duplicateList = "";
            List<UserData> ud = new List<UserData>();
            foreach (var elem in xml.Root.Elements("tbUser"))
            {
                var parsed = TryParseUserFromXElement(elem, ref failedList);
                if (parsed != null)
                {
                    if (!ud.Select(o => o.UserName).Contains(parsed.UserName))
                        ud.Add(parsed);
                    else
                        duplicateList += " " + parsed.UserName;
                }
            }

            if (!string.IsNullOrEmpty(failedList))
                errorMessage = "TryParseUserFromXElement: Impossible d'ajouter les utilisateurs " + failedList;
            if (!string.IsNullOrEmpty(duplicateList))
                errorMessage = "/!\\ Il existe des utilisateurs en double.<br/> Les doublons ont été ignorés : " + duplicateList;

            failedList = "";
            // Pas de AddRange pour avoir des erreurs détaillées
            foreach (var user in ud)
            {
                try {
                    if (_context.UserData.Any(o => o.UserName == user.UserName))
                        continue;
                    await _context.UserData.AddAsync(user);
                }
                catch
                {
                    failedList += " \"" + user.UserName + " \"";
                }
            }
            var saveContextError = await SaveContext();
            if (!string.IsNullOrEmpty(saveContextError))
                return saveContextError;

            if (!string.IsNullOrEmpty(failedList))
                errorMessage += "<br/><br/>_context.UserData.AddAsync: Impossible d'ajouter les utilisateurs " + failedList;

            failedList = "";
            var userDataEvaluated = _context.UserData.ToList();
            foreach (var userData in userDataEvaluated)
            {
                if (await _userManager.FindByNameAsync(userData.UserName) != null)
                    continue;

                var user = new IdentityUserDefaultPwd
                {
                    UserName = userData.UserName
                };

                try {
                    var ir = await _userManager.CreateAsync(user, IdentityUserDefaultPwd.DefaultPassword);
                    if (!ir.Succeeded)
                    {
                        failedList += " \"" + user.UserName + " \"";
                        _context.UserData.Remove(ud.First(o => o.UserName == user.UserName));
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user,
                            "PG");
                    }
                }
                catch (Exception e) {
                    errorMessage += "Erreur interne à Identity : " + e;
                }
                
            }
            saveContextError = await SaveContext();
            if (!string.IsNullOrEmpty(saveContextError))
                return saveContextError;

            if (!string.IsNullOrEmpty(failedList))
                errorMessage += "<br/><br/>Identity: Impossible de créer les comptes pour les utilisateurs " + failedList;
                
            return "0" + errorMessage;
        }
    }
}
