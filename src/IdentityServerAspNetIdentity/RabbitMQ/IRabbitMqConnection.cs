using RabbitMQ.Client;

namespace IdentityServerAspNetIdentity.RabbitMQ;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
