using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Pages.Account.Registration;

public class InputModel
{

    [Required]
    public string? Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    public string? ReturnUrl { get; set; }

    public string? Button { get; set; }
}
