using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitProducer
{
    public class DirectExchangePublisher
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("contoh-direct-exchange", ExchangeType.Direct);
            var count = 0;
            while (true)
            {
                var message = new { Name = "Producer", Message = $"Urutan pesan ke-: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                channel.BasicPublish("contoh-direct-exchange", "katalog.barang", null, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}