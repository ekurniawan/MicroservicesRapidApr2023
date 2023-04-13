using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitSubscriber
{
    public class DirectExchageConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("contoh-direct-exchange", ExchangeType.Direct);
            channel.QueueDeclare("contoh-direct-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("contoh-direct-queue", "contoh-direct-exchange", "katalog.barang");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("contoh-direct-queue", true, consumer);
            Console.WriteLine("Aplikasi consumer dijalankan...");
            Console.ReadLine();
        }
    }
}