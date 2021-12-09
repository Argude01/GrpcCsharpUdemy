using Calculator;
using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.IO;
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
                Console.WriteLine();
            });

            // Testing GreetingService
            // ==================================================================

            //var client = new DummyService.DummyServiceClient(channel);
            var client = new GreetingService.GreetingServiceClient(channel);

            //DoSimpleGreet(client); 
            //await DoManyGreetings(client);
            //await DoLongGreet(client);
             await DoGreetEveryone(client);

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

        // 1) --- Unary API
        public static void DoSimpleGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Deyanira",
                LastName = "Gutierrez"
            };

            var requestUny = new GreetingRequest() { GreetingUny = greeting };

            var responseUny = client.Greet(requestUny);
            Console.WriteLine("ResponseUny: " + responseUny.Result);
            Console.WriteLine();
        }

        // 2) --- Stream API SERVER
        public static async Task DoManyGreetings(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Deyanira",
                LastName = "Gutierrez"
            };

            var requestStreamServer = new GreetingManyTimesRequest { GreetingMany = greeting };
            var responseMany = client.GreetManyTimes(requestStreamServer);

            while (await responseMany.ResponseStream.MoveNext())
            {
                Console.WriteLine(responseMany.ResponseStream.Current.Result);
                await Task.Delay(200);
            }
        }

        // 3) --- Stream API CLIENT
        public static async Task DoLongGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Deyanira",
                LastName = "Gutierrez"
            };

            var requestStreamClient = new LongGreetingRequest { LongGreeting = greeting };
            var stream = client.LongGreet();

            foreach (int i in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(requestStreamClient);
            }

            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine(response.Result);
        }    

        // 4) --- Bidi Stream API
        public static async Task DoGreetEveryone(GreetingService.GreetingServiceClient client)
        {
            var stream = client.GreetEveryone();

            var responseReaderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("RECEIVED - Response-Bidi: " + stream.ResponseStream.Current.Result);
                }
            });

            Greeting[] greetings =
            {
                new Greeting() { FirstName = "Deyanira", LastName = "Gutierrez" },
                new Greeting() { FirstName = "Deyanira", LastName = "Gutierrez" },
                new Greeting() { FirstName = "Deyanira", LastName = "Gutierrez" }
            };

            foreach (var greeting in greetings)
            {
                Console.WriteLine("Sending : " + greeting.ToString());
                await stream.RequestStream.WriteAsync(new GreetingEveryoneRequest()
                {
                    GreetingEveryone = greeting
                });
            }

            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;
        }
    }
}
