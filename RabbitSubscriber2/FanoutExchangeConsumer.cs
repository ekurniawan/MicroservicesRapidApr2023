using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitSubscriber2
{
    public class FanoutExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout);
            channel.QueueDeclare("demo-fanout-queue2",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("demo-fanout-queue2", "demo-fanout-exchange", string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-fanout-queue2", true, consumer);
            Console.WriteLine("Consumer Started");
            Console.ReadLine();
        }
    }
}