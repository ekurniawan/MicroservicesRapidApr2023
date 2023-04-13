// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitProducer;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
DirectExchangePublisher.Publish(channel);
