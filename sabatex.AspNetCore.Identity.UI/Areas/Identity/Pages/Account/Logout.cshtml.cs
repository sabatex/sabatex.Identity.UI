// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using sabatex.AspNetCore.Identity.UI.Areas.Identity.Pages;

namespace sabatex.AspNetCore.Identity.UI.Pages.Account.Internal
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    [IdentityDefaultUI(typeof(LogoutModel<>))]
    public abstract class LogoutModel : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<IActionResult> OnPost(string returnUrl = null) => throw new NotImplementedException();
    }

    internal class LogoutModel<TUser> : LogoutModel where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IStringLocalizer _localizer;

        public LogoutModel(SignInManager<TUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public override async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(LoggerEventIds.UserLoggedOut, "User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
