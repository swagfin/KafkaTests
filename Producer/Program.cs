using Confluent.Kafka;
using System;

namespace Producer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Kafka broker endpoint
            var brokerEndpoint = "172.20.81.103:9092";
            // Topic to send messages to
            var topicName = "my-topic";
            // Create producer configuration
            var config = new ProducerConfig
            {
                BootstrapServers = brokerEndpoint
            };
            // Create producer client
            // Kafka, messages are sent to topics as key-value pairs
            // in this case, Null is used where there is no Key required
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    while (true)
                    {
                        Console.WriteLine("Send Message");
                        // Create a message to send
                        var message = new Message<Null, string>
                        {
                            Value = Console.ReadLine()
                        };
                        // Send the message to the topic
                        producer.ProduceAsync(topicName, message).GetAwaiter().GetResult();
                        Console.WriteLine($"Sent message to topic {topicName}: {message.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }


            Console.WriteLine(Console.ReadLine());
        }
    }
}
