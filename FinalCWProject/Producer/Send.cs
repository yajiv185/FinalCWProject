using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RabbitMqPublishing;

public class Produce
{
    byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter binaryFormat = new BinaryFormatter();
        using (MemoryStream memStream = new MemoryStream())
        {
            binaryFormat.Serialize(memStream, obj);
            return memStream.ToArray();
        }
    }

    public void sender(int stockId, string imgUrl)
    {
        var factory = new ConnectionFactory() { HostName = "172.16.0.11" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "stockimage_g3",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            for (int i = 0; i < 1; i++)
            {
                NameValueCollection nvc = new NameValueCollection();
                string id = stockId.ToString();
                nvc.Add(id, imgUrl);
                byte[] bytenvc = ObjectToByteArray(nvc);
                channel.BasicPublish(exchange: "",
                                   routingKey: "stockimage_g3",
                                   basicProperties: null,
                                   body: bytenvc);
            }
        }
    }
}