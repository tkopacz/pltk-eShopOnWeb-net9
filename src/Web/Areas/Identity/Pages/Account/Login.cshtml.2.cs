#if NETCOREAPP2_0

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IBasketService _basketService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, IBasketService basketService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _basketService = basketService;
            Input = new InputModel();
            ExternalLogins = new List<AuthenticationScheme>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = Input != null ? Input.Email : null;
            var password = Input != null ? Input.Password : null;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return Page();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(email, password, Input.RememberMe, true);

            if (signInResult.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                await TransferAnonymousBasketToUserAsync(email);
                return LocalRedirect(ReturnUrl);
            }

            if (signInResult.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = ReturnUrl, RememberMe = Input.RememberMe });
            }

            if (signInResult.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        private async Task TransferAnonymousBasketToUserAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                var anonymousId = Request.Cookies[Constants.BASKET_COOKIENAME];
                Guid parsedValue;
                if (Guid.TryParse(anonymousId, out parsedValue))
                {
                    Guard.Against.NullOrEmpty(userName, nameof(userName));
                    await _basketService.TransferBasketAsync(anonymousId, userName);
                }

                Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
            }
        }
    }
}

#endif
