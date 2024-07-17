using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Account.ConfirmEmail
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
