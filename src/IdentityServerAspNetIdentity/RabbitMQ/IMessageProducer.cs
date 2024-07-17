using NotificationService.API;

namespace IdentityServerAspNetIdentity.RabbitMQ;

public interface IMessageProducer
{
    public void SendEmailConfirmationMessage(EmailConfirmationMessage message); 
}
