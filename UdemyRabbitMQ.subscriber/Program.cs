using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

await using var connection = await factory.CreateConnectionAsync();

await using var channel = await connection.CreateChannelAsync();

//await channel.QueueDeclareAsync("hello-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

var consumer = new AsyncEventingBasicConsumer(channel);
channel.BasicConsumeAsync("hello-queue", true, consumer);

consumer.ReceivedAsync += (object sender, BasicDeliverEventArgs @event) =>
{
    var message = Encoding.UTF8.GetString(@event.Body.ToArray());
    Console.WriteLine("Message: " + message);
    return Task.CompletedTask;
};

Console.ReadLine();