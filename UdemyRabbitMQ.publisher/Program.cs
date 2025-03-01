using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

var factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672")
};

await using var connection = await factory.CreateConnectionAsync();

await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("hello-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

string message = "Hello RabbitMQ";
var messageBody = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(string.Empty, "hello-queue", body: messageBody.AsMemory());

Console.WriteLine("Message is sent");
Console.ReadLine();