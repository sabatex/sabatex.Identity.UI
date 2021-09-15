using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sabatex.AspNetCore.Identity.UI.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendConfirmEmailAsync(string email, string callbackUrl);
}

