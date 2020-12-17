using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using iSy.Models;
using iSy.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using isyAuth = iSy.Services.IAuthenticationService;

namespace iSy.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly isyAuth _authService;

        public LoginModel(ILogger<LoginModel> logger, isyAuth authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync(string paramUsername, string paramPassword, string target)
        {
            string returnUrl = Url.Content($"~{target}");

            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error signing out: {ex.Message}");
            }

            LdapAuthUser user;
            try
            {
                user = await _authService.Login(paramUsername, paramPassword);
            }
            catch (Exception ex)
            {
                var msg = $"Error signing in: {ex.Message}";
                _logger.LogError(ex, msg);

                if(returnUrl.Contains("?"))
                {
                    returnUrl += "&";
                } else
                {
                    returnUrl += "?";
                }

                return LocalRedirect($"{returnUrl}emsg={ex.Message}");
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim("EmployeeId", user.EmployeeId),
                new Claim("CompanyKey", user.CompanyKey)
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Request.Host.Value
            };

            try
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error signing in: {ex.Message}");
            }

            return LocalRedirect(returnUrl);
        }
    }
}
