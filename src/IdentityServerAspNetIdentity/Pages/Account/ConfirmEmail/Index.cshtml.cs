using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Account.ConfirmEmail;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(string token, string email, string returnUrl)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Redirect("/Account/Confirmemail/Error");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded ? Page() : Redirect("/Account/Confirmemail/Error");
    }
}
