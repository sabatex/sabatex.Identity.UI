// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using sabatex.AspNetCore.Identity.UI.Areas.Identity.Pages;

namespace sabatex.AspNetCore.Identity.UI.Pages.Account.Internal
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    [IdentityDefaultUI(typeof(ConfirmEmailChangeModel<>))]
    public abstract class ConfirmEmailChangeModel : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Task<IActionResult> OnGetAsync(string userId, string email, string code) => throw new NotImplementedException();
    }

    internal class ConfirmEmailChangeModel<TUser> : ConfirmEmailChangeModel where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly IStringLocalizer _localizer;

        public ConfirmEmailChangeModel(UserManager<TUser> userManager,
                                       SignInManager<TUser> signInManager,
                                       IStringLocalizerFactory localizerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizerFactory.Create(SharedResource.ResourceType);
        }

        public override async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(_localizer["Unable to load user with ID '{0}'.",userId]);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer["Error changing email."];
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = _localizer["Error changing user name."];
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["Thank you for confirming your email change."];
            return Page();
        }
    }
}
