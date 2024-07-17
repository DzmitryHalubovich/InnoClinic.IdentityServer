﻿using InnoClinic.SharedModels.MQMessages.IdentityServer;

namespace IdentityServerAspNetIdentity.RabbitMQ;

public interface IMessageProducer
{
    public void SendEmailConfirmationMessage(EmailConfirmationMessage message); 
}
