using NewConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

class Receive
{
    Object ByteArrayToObject(byte[] byteStream)
    {
        if (byteStream == null)
            return null;
        BinaryFormatter binaryFormat = new BinaryFormatter();
        using (MemoryStream memStream = new MemoryStream(byteStream))
        {
            Object imgObject = binaryFormat.Deserialize(memStream);
            return imgObject;
        }
    }

    public void Receiver()
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

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                //call
                NameValueCollection val = (NameValueCollection)ByteArrayToObject(body);
                // retreiving image attributes
                string imagePath = val.Get(0);
                int stockId = Convert.ToInt32(val.GetKey(0));
                ImageProcess ip = new ImageProcess();
                ip.SaveImage(imagePath, stockId);



                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: "stockimage_g3_2",
                                 noAck: false,
                                 consumer: consumer);
           
            Console.ReadLine();
        }
    }
}