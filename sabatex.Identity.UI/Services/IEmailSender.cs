using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sabatex.AspNetCore.Identity.UI.Services;

public interface IEmailSender
{
    /// <summary>
    /// Send email async
    /// </summary>
    /// <param name="email">email adderess</param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <returns>emptyString if success or error message </returns>
    Task<string> SendEmailAsync(string email, string subject, string message);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="callbackUrl"></param>
    /// <returns>emptyString if success or error message </returns>
    Task<string> SendConfirmEmailAsync(string email, string callbackUrl);
}

