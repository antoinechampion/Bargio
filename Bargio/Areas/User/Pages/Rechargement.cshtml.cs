//          Bargio - Rechargement.cshtml.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Bargio.Areas.User.Pages
{
    public class RechargementModel : PageModel
    {
        // Active l'API de test de lydia m�me sur le site en ligne
        private const bool ForceTestApi = false;
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _lydiaApiUrl;

        private readonly string _lydiaVendorToken;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;

        public RechargementModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager,
            IHostingEnvironment env) {
            _context = context;
            _userManager = userManager;
            // En fonction de l'environnement, on charge les donnees de test ou de production
            _lydiaVendorToken = env.IsDevelopment() || ForceTestApi
                ? "5bd083bec025c852794717"
                : "5774f7d9df76f082807252";
            _lydiaApiUrl = env.IsDevelopment() || ForceTestApi
                ? "https://homologation.lydia-app.com/api/request/do.json"
                : "https://lydia-app.com/api/request/do.json";

            var systemParameters = _context.SystemParameters.First();
            CommissionLydiaVariable = systemParameters.CommissionLydiaVariable;
            CommissionLydiaFixe = systemParameters.CommissionLydiaFixe;
            MinimumRechargementLydia = systemParameters.MinimumRechargementLydia;
        }

        [BindProperty]
        [Required]
        [Range(1, double.PositiveInfinity, ErrorMessage = "Veuillez entrer un solde correct")]
        [DataType(DataType.Currency, ErrorMessage = "Veuillez entrer un solde correct")]
        [Display(Name = "Montant en �")]
        public decimal Montant { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Veuillez entrer un num�ro de t�l�phone valide")]
        [Display(Name = "N� de t�l�phone")]
        public string Telephone { get; set; }

        [BindProperty] public decimal SoldeActuel { get; set; }

        [BindProperty] public string StatutPaiement { get; set; }

        [BindProperty] public string ClasseTexteStatut { get; set; }

        [BindProperty] public decimal CommissionLydiaVariable { get; set; }

        [BindProperty] public decimal CommissionLydiaFixe { get; set; }

        [BindProperty] public decimal MinimumRechargementLydia { get; set; }

        // Si succ�s, retourne (true, url_de_paiement), sinon retourne (false, msg_erreur)
        // Passage en prod : changer PublicTestToken en PublicToken, changer testApiUrl en apiUrl
        // https://homologation.lydia-app.com/index.php/backoffice/request/index
        public async Task<(bool, string)> LydiaInitiatePayment(string id, decimal montantPaye) {
            var req = Url.ActionContext.HttpContext.Request;
            var absoluteUri = req.Scheme + "://" + req.Host;

            var postData = new LydiaRequestData {
                VendorToken = _lydiaVendorToken,
                Recipient = Telephone,
                Amount = montantPaye.ToString("0.##", new CultureInfo("en-US")),
                OrderRef = id,
                ConfirmUrl = absoluteUri + "/api/lydia/confirm",
                CancelUrl = absoluteUri + "/api/lydia/cancel",
                ExpireUrl = absoluteUri + "/api/lydia/cancel",
                EndMobileUrl = absoluteUri + "/user/rechargement?statut=succes",
                BrowserSuccessUrl = absoluteUri + "/user/rechargement?statut=succes",
                BrowserFailUrl = absoluteUri + "/user/rechargement?statut=echec"
            };
            var json = JsonConvert.SerializeObject(postData);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var content = new FormUrlEncodedContent(dict);
            var resp = await _httpClient.PostAsync(_lydiaApiUrl, content);
            dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
            if (o.error == "0")
                return (true, o.mobile_url);
            return (false, o.message);
        }

        public async Task<string> CreatePaymentRequest() {
            var paymentRequest = new PaymentRequest {
                Montant = Montant,
                UserName = _userManager.GetUserName(User)
            };
            _context.PaymentRequest.Add(paymentRequest);
            await _context.SaveChangesAsync();
            return paymentRequest.ID;
        }

        public async Task<IActionResult> OnGet(string statut) {
            if (_context.SystemParameters.First().LydiaBloque) {
                return Redirect("/Error/531");
            }
            if (statut == "succes") {
                ClasseTexteStatut = "text-success";
                StatutPaiement = "Paiement effectu�";
            }
            else if (statut == "echec") {
                ClasseTexteStatut = "text-danger";
                StatutPaiement = "Impossible d'effectuer le paiement";
            }
            else {
                ClasseTexteStatut = "d-none";
            }

            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            SoldeActuel = _context.UserData.Find(identityUser.UserName).Solde;
            return Page();
        }

        public async Task<IActionResult> OnPost() {
            if (_context.SystemParameters.First().LydiaBloque) {
                return Redirect("/Error/531");
            }
            if (!ModelState.IsValid) return Page();

            var minimumRechargement = _context.SystemParameters.First().MinimumRechargementLydia;
            if (Montant < minimumRechargement) {
                ModelState.AddModelError(string.Empty,
                    "Le minimum de rechargement est de " + minimumRechargement + "�.");
                return Page();
            }

            var montantPaye = (Montant + CommissionLydiaFixe * 1.2M) / (1 - 1.2M * CommissionLydiaVariable / 100);
            var id = await CreatePaymentRequest();
            var (success, message) = await LydiaInitiatePayment(id, montantPaye);
            if (success) return Redirect(message);
            ModelState.AddModelError(string.Empty, "Echec. " + message);
            return Page();
        }

        private class LydiaRequestData
        {
            // API public key
            [JsonProperty("vendor_token")] public string VendorToken { get; set; }

            // N� Tel
            [JsonProperty("recipient")] public string Recipient { get; set; }

            // Message de demande
            [JsonProperty("message")] public string Message { get; } = "Rechargement compte Foy'ss";

            // Prix
            [JsonProperty("amount")] public string Amount { get; set; }

            // Monnaie
            [JsonProperty("currency")] public string Currency { get; } = "EUR";

            // Par tel ou email
            [JsonProperty("type")] public string Type { get; } = "phone";

            // Id transaction
            [JsonProperty("order_ref")] public string OrderRef { get; set; }

            // Message de confirmation
            [JsonProperty("display_confirmation")] public string DisplayConfirmation { get; } = "0";

            // Methode de paiement (CB interdites)
            [JsonProperty("payment_method")] public string PaymentMethod { get; } = "lydia";

            // Callback c�t� serveur si le paiement est reussi
            [JsonProperty("confirm_url")] public string ConfirmUrl { get; set; }

            // Callback c�t� serveur si le paiement a �t� annul�
            [JsonProperty("cancel_url")] public string CancelUrl { get; set; }

            // Callback c�t� serveur si le paiement a expir�
            [JsonProperty("expire_url")] public string ExpireUrl { get; set; }

            // Url de redirection pour mobiles
            [JsonProperty("end_mobile_url")] public string EndMobileUrl { get; set; }

            // Url de redirection pour le web
            [JsonProperty("browser_success_url")] public string BrowserSuccessUrl { get; set; }

            // Url de redirection pour le web en cas d'�chec
            [JsonProperty("browser_fail_url")] public string BrowserFailUrl { get; set; }

            // Renvoyer directement l'url de paiement
            [JsonProperty("delayed_payment")] public string DelayedPayment { get; set; } = "0";
        }
    }
}