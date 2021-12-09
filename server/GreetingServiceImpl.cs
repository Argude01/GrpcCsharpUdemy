using System;
using System.Linq;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;
using static Greet.GreetingService;

namespace server
{
    public class GreetingServiceImpl : GreetingServiceBase
    {
        // Unary API implementation
        public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = String.Format("hello {0} {1}", request.GreetingUny.FirstName, request.GreetingUny.LastName);

            return Task.FromResult(new GreetingResponse() { Result = result });
        }

        // Server Stream API implementation
        public override async Task GreetManyTimes(GreetingManyTimesRequest request, IServerStreamWriter<GreetingManyTimesReponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("The server received the request: ");
            Console.WriteLine(request.ToString());

            string result = String.Format("GreetManyTimes: {0} {1}.", request.GreetingMany.FirstName, request.GreetingMany.LastName);

            foreach (int i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(new GreetingManyTimesReponse() { Result = result });
            }
        }

        // Client Stream API implementation
        public override async Task<LongGreetingResponse> LongGreet(IAsyncStreamReader<LongGreetingRequest> requestStream, ServerCallContext context)
        {
            string result = "";

            while(await requestStream.MoveNext())
            {
                result += String.Format("LongGreet: {0} {1} {2}",
                    requestStream.Current.LongGreeting.FirstName,
                    requestStream.Current.LongGreeting.LastName,
                    Environment.NewLine);
            }

            return new LongGreetingResponse() { Result = result };  
        }

        // Bidi Stream API implementation
        public override async Task GreetEveryone(IAsyncStreamReader<GreetingEveryoneRequest> requestStream, IServerStreamWriter<GreetingEveryoneResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var result = String.Format("Hello {0} {1}",
                                            requestStream.Current.GreetingEveryone.FirstName,
                                            requestStream.Current.GreetingEveryone.LastName
                                            );

                Console.WriteLine("Sending : " + result);
                await responseStream.WriteAsync(new GreetingEveryoneResponse() { Result = result });
            }
        }
    }
}
