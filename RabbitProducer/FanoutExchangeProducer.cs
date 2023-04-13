using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitProducer
{
    public class FanoutExchangeProducer
    {
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-fanout-exchange",
            ExchangeType.Fanout);

            var count = 0;
            while (true)
            {
                var message = new { Name = "Producer ", Message = $"pesan yang dikirimkan : {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                channel.BasicPublish("demo-fanout-exchange", string.Empty, null, body);
                count++;
                Thread.Sleep(1000);
            }

        }
    }
}