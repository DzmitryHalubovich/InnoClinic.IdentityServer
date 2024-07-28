using IdentityServerAspNetIdentity.Models;
using InnoClinic.SharedModels.MQMessages.IdentityServer;
using InnoClinic.SharedModels.MQMessages.Profiles;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace IdentityServerAspNetIdentity.MassTransit
{
    public class WorkerProfileCreatedConsumer : IConsumer<WorkerProfileCreatedMessage>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;

        public WorkerProfileCreatedConsumer(UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<WorkerProfileCreatedMessage> context)
        {
            var message = context.Message;

            var user = new ApplicationUser()
            {
                UserName = message.Email,
                Email = message.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, message.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, message.Role);

                await _publishEndpoint.Publish<WorkerProfileRegisteredMessage>(new()
                {
                    Email = message.Email,
                    Password = message.Password
                });
            }

            foreach (var error in result.Errors)
            {
                Log.Error(error.Description);
            }
        }
    }
}
