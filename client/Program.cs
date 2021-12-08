using Calculator;
using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace client
{
    internal class Program
    {
        const string target = "127.0.0.1:50051";
        static async Task Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            // Testing GreetingService
            // ================================================================

            //var client = new DummyService.DummyServiceClient(channel);
            var client = new GreetingService.GreetingServiceClient(channel);

            var greeting = new Greeting()
            {
                FirstName = "Deyanira",
                LastName = "Gutierrez"
            };

            // --- Unary API
            //var request = new GreetingRequest() { GreetingUny = greeting };
            
            //var response = client.Greet(request);
            //Console.WriteLine("Response: " + response.Result);

            // --- Stream API SERVER
            //var request = new GreetingManyTimesRequest { GreetingMany = greeting };
            //var response = client.GreetManyTimes(request);

            //while (await response.ResponseStream.MoveNext())
            //{
            //    Console.WriteLine(response.ResponseStream.Current.Result);
            //    await Task.Delay(200);
            //}

            // --- Stream API CLIENT
            var request  = new LongGreetingRequest { LongGreeting = greeting };
            var stream = client.LongGreet();

            foreach (int i in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(request);
            }

            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine(response.Result);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();

            
            // CalculatorService
            // ===================================================================
            Channel calculatorChannel = new Channel(target, ChannelCredentials.Insecure);
            var calculatorClient = new CalculatorService.CalculatorServiceClient(calculatorChannel);

            var requestCalculator = new OperationRequest()
            {
                Number1 = 10,
                Number2 = 3
            };

            var responseCalculator = calculatorClient.Sum(requestCalculator);
            Console.WriteLine(responseCalculator.Result);
            calculatorChannel.ShutdownAsync().Wait();
            Console.ReadKey();  
        }
    }
}
