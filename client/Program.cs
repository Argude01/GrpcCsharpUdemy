using Blog;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("localhost", 50052, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            // CreateBlog rpc method
            //var clientCreate = new BlogService.BlogServiceClient(channel);
            //var responseCreate = clientCreate.CreateBlog(new CreateBlogRequest
            //{
            //    Blog = new Blog.Blog()
            //    {
            //        AuthorId =  "Clement",
            //        Title = "New Blog!",
            //        Content = "Hello world, this is a new blog."
            //    }
            //});

            //Console.WriteLine("The blog " + responseCreate.Blog.Id + " was created !");
            //channel.ShutdownAsync().Wait();
            //Console.ReadKey();


            // ReadBlog rpc method
            var client = new BlogService.BlogServiceClient(channel);

            try
            {
                var response = client.ReadBlog(new ReadBlogRequest()
                {
                    BlogId = "61cb215c94427875cf2e6bbc"
                });
                Console.WriteLine("Response: " + response.Blog.ToString());
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }

            channel.ShutdownAsync().Wait();
            Console.ReadKey();

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
