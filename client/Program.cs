using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    internal class Program
    {
        const string target = "127.0.0.1:50051";
        static void Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            // Testing GreetingService
            //var client = new DummyService.DummyServiceClient(channel);
            //var client = new GreetingService.GreetingServiceClient(channel);

            //var greeting = new Greeting()
            //{
            //    FirstName = "Deyanira",
            //    LastName = "Gutierrez"
            //};

            //var request = new GreetingRequest() { Greeting = greeting };
            //var response = client.Greet(request);

            //Console.WriteLine("Response: " + response.Result);
            //channel.ShutdownAsync().Wait();
            //Console.ReadKey();

            // CalculatorService
            //Channel calculatorChannel = new Channel(target, ChannelCredentials.Insecure);
            //var calculatorClient = new CalculatorService.CalculatorServiceClient(calculatorChannel);

            //var requestCalculator = new OperationRequest()
            //{
            //    Number1 = 10,
            //    Number2 = 3
            //};

            //var responseCalculator = calculatorClient.Sum(requestCalculator);
            //Console.WriteLine(responseCalculator.Result);
            //calculatorChannel.ShutdownAsync().Wait();
            //Console.ReadKey();  
        }
    }
}
