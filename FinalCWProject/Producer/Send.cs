using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RabbitMqPublishing;
using System.Configuration;

public class Produce
{
    private static byte[] ObjectToByteArray(object obj)
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

    public static void Sender(int stockId, string imgUrl)
    {
        var factory = new ConnectionFactory() { HostName = ConfigurationManager.AppSettings["RabbitMQ"] };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "stockimage_g3_2",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            NameValueCollection nvc = new NameValueCollection();
            string id = stockId.ToString();
            nvc.Add(id, imgUrl);
            byte[] bytenvc = ObjectToByteArray(nvc);
            channel.BasicPublish(exchange: "",
                               routingKey: "stockimage_g3_2",
                               basicProperties: null,
                               body: bytenvc);
        }
    }
}