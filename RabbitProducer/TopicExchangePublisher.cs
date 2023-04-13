using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitProducer
{
    public class TopicExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("contoh-topic-exchange",
            ExchangeType.Topic);

            var count = 0;
            while (true)
            {
                var message = new { Name = "Producer", Message = $"Urutan pesan ke: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                channel.BasicPublish("contoh-topic-exchange", "katalog.details", null, body);
                count++;
                Thread.Sleep(1000);
            }

        }
    }
}