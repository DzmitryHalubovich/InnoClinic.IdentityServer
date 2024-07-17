using Newtonsoft.Json;
using NotificationService.API;
using RabbitMQ.Client;
using System.Text;

namespace IdentityServerAspNetIdentity.RabbitMQ;

public class MessageProducer : IMessageProducer
{
    private readonly IRabbitMqConnection _connection;

    public MessageProducer(IRabbitMqConnection connection)
    {
        _connection = connection;
    }

    public void SendEmailConfirmationMessage(EmailConfirmationMessage message)
    {
        using var channel = _connection.Connection.CreateModel();

        var queueName = "Registration";

        var exchangeName = "email-confirm-queue";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: queueName);

        var json = JsonConvert.SerializeObject(message);

        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    }
}
