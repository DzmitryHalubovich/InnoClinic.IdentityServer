using RabbitMQ.Client;

namespace IdentityServerAspNetIdentity.RabbitMQ;

public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private IConnection? _connection;
    private IConfiguration _configuration;

    public IConnection Connection => _connection;

    public RabbitMqConnection(IConfiguration configuration)
    {
        _configuration = configuration;

        InitializeConnection();
    }

    private void InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration.GetSection("RabbitMQ:HostName").Value
        };

        _connection = factory.CreateConnection();

        //EnsureQueuesExist(_connection);
    }

    private void EnsureQueuesExist(IConnection connection)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "Registration",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
