using AutoMapper;
using IdentityServerAspNetIdentity.Models;
using InnoClinic.SharedModels.MQMessages.IdentityServer;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Account.Registration;

[SecurityHeaders]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IPublishEndpoint _messageProducer;
    private readonly IMapper _mapper;

    public Index(
        UserManager<ApplicationUser> userManager,
        IPublishEndpoint messageProducer,
        IMapper mapper)
    {
        _messageProducer = messageProducer;
        _userManager = userManager;
        _mapper = mapper;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPost(InputModel input)
    {
        if (input.Button != "registration")
        {
            return Redirect(input.ReturnUrl ?? "~/");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = _mapper.Map<ApplicationUser>(input);

        var result = await _userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return Page();
        }

        await SendEmailConfirmMessage(user);

        await _userManager.AddToRoleAsync(user, "Patient");

        return Redirect(input.ReturnUrl ?? "~/");
    }

    private async Task SendEmailConfirmMessage(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Page("/Account/ConfirmEmail/Index", pageHandler: null, new { token, email = user.Email }, protocol: Request.Scheme);

        await _messageProducer.Publish<EmailConfirmationMessage>(new ()
        {
            Email = user.Email,
            ConfirmationLink = confirmationLink
        });
    }
}
