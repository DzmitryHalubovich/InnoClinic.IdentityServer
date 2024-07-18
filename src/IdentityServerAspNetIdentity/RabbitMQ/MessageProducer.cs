using InnoClinic.SharedModels.MQMessages.IdentityServer;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

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

        var queueName = "email-confirm-queue";
        var routigKey = "email.command.confirm";
        var exchangeName = "registration";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueDeclare(queue: queueName,
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

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
