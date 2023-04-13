using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitSubscriber
{
    public class TopicExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("contoh-topic-exchange", ExchangeType.Topic);
            channel.QueueDeclare("contoh-topic-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("contoh-topic-queue", "contoh-topic-exchange", "katalog.*");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("contoh-topic-queue", true, consumer);
            Console.WriteLine("Aplikasi consumer dijalankan...");
            Console.ReadLine();
        }
    }
}