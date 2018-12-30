using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bargio.Areas.Identity;
using Bargio.Data;
using Bargio.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Bargio.Areas.User.Pages
{
    public class RechargementModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserDefaultPwd> _userManager;
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string _lydiaVendorToken;
        private readonly string _lydiaApiUrl;

        // Active l'API de test de lydia même sur le site en ligne
        private const bool ForceTestApi = true;

        public RechargementModel(ApplicationDbContext context, UserManager<IdentityUserDefaultPwd> userManager,
                IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            // En fonction de l'environnement, on charge les donnees de test ou de production
            _lydiaVendorToken = (env.IsDevelopment() || ForceTestApi) ? "5bd083bec025c852794717" : "5bd083bec025c852794717";
            _lydiaApiUrl = (env.IsDevelopment() || ForceTestApi)
                ? "https://homologation.lydia-app.com/api/request/do.json"
                : "https://lydia-app.com/api/request/do.json";
        }

        [BindProperty]
        [Required]
        [Range(1, Double.PositiveInfinity, ErrorMessage = "Veuillez entrer un solde correct")]
        [DataType(DataType.Currency, ErrorMessage = "Veuillez entrer un solde correct")]
        [Display(Name = "Montant en €")]
        public decimal Montant { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Veuillez entrer un numéro de téléphone valide")]
        [Display(Name = "N° de téléphone")]
        public string Telephone { get; set; }
      

        private class LydiaRequestData
        {
            // API public key
            [JsonProperty("vendor_token")] public string VendorToken { get; set; }

            // N° Tel
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

            // Callback côté serveur si le paiement est reussi
            [JsonProperty("confirm_url")] public string ConfirmUrl { get; set; }

            // Callback côté serveur si le paiement a été annulé
            [JsonProperty("cancel_url")] public string CancelUrl { get; set; }

            // Callback côté serveur si le paiement a expiré
            [JsonProperty("expire_url")] public string ExpireUrl { get; set; }

            // Url de redirection pour mobiles
            [JsonProperty("end_mobile_url")] public string EndMobileUrl { get; set; }

            // Url de redirection pour le web
            [JsonProperty("browser_success_url")] public string BrowserSuccessUrl { get; set; }

            // Url de redirection pour le web en cas d'échec
            [JsonProperty("browser_fail_url")] public string BrowserFailUrl { get; set; }

            // Renvoyer directement l'url de paiement
            [JsonProperty("delayed_payment")] public string DelayedPayment { get; set; } = "0";
        }

        // Si succès, retourne (true, url_de_paiement), sinon retourne (false, msg_erreur)
        // Passage en prod : changer PublicTestToken en PublicToken, changer testApiUrl en apiUrl
        // https://homologation.lydia-app.com/index.php/backoffice/request/index
        public async Task<(bool,string)> LydiaInitiatePayment(string id)
        {
            var req = Url.ActionContext.HttpContext.Request;
            var absoluteUri = req.Scheme + "://" + req.Host;

            var postData = new LydiaRequestData
            {
                VendorToken = _lydiaVendorToken,
                Recipient = Telephone,
                Amount = Montant.ToString("0.##"),
                OrderRef = id,
                ConfirmUrl = absoluteUri + "/api/lydia/confirm",
                CancelUrl = absoluteUri + "/api/lydia/cancel",
                ExpireUrl = absoluteUri + "/api/lydia/cancel",
                EndMobileUrl = absoluteUri + "/user/paiementsucces",
                BrowserSuccessUrl = absoluteUri + "/user/paiementsucces",
                BrowserFailUrl = absoluteUri + "/user/paiementechec",
            };
            var json = JsonConvert.SerializeObject(postData);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var content = new FormUrlEncodedContent(dict);
            var resp = await _httpClient.PostAsync(_lydiaApiUrl, content);
            dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
            if (o.error == "0") { 
                return (true, o.mobile_url);
            }
            else
            {
                return (false, o.message);
            }
        }

        public async Task<string> CreatePaymentRequest()
        {
            var paymentRequest = new PaymentRequest
            {
                Montant = Montant,
                UserName = _userManager.GetUserName(User)
            };
            _context.PaymentRequest.Add(paymentRequest);
            await _context.SaveChangesAsync();
            return paymentRequest.ID;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = await CreatePaymentRequest();
            var (success, message) = await LydiaInitiatePayment(id);
            if (success)
            {
                return Redirect(message);
            }
            ModelState.AddModelError(string.Empty, "Echec. " + message);
            return Page();
        }
    }
}