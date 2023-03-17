using Confluent.Kafka;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Kafka broker endpoint
            var brokerEndpoint = "localhost:9092";
            // Topic to consume messages from
            var topicName = "my-topic";
            // Create consumer configuration
            var config = new ConsumerConfig
            {
                BootstrapServers = brokerEndpoint,
                GroupId = "my-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            // Create consumer client
            using (var consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build())
            {
                // Subscribe to the topic
                consumer.Subscribe(topicName);

                // Start consuming messages in a background thread
                var cancellationToken = new CancellationTokenSource();
                var task = Task.Factory.StartNew(() =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            // Poll for a new message from the topic
                            ConsumeResult<Ignore, byte[]> consumedObj = consumer.Consume(cancellationToken.Token);
                            // Deserialize the message value from a byte array to a string
                            var json = Encoding.UTF8.GetString(consumedObj.Message.Value);
                            // Print the received message
                            Console.WriteLine($"Received message from topic {consumedObj.TopicPartition}: {json}");
                        }
                        catch (OperationCanceledException)
                        {
                            // Graceful shutdown when the cancellation token is triggered
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }, cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                // Wait for the user to press a key to stop consuming messages
                Console.WriteLine("Press any key to stop consuming messages...");
                Console.ReadKey();

                // Cancel the consuming task and wait for it to complete
                cancellationToken.Cancel();
                task.Wait();
            }
        }
    }
}
