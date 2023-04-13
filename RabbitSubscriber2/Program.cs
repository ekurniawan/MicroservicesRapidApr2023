﻿using RabbitMQ.Client;
using RabbitSubscriber2;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

FanoutExchangeConsumer.Consume(channel);