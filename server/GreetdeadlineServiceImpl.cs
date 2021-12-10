using Greetdeadline;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Greetdeadline.GreetdeadlineService;

namespace server
{
    public class GreetdeadlineServiceImpl : GreetdeadlineServiceBase
    {
        public override async Task<GreetdeadlineResponse> Greetdeadline(GreetdeadlineRequest request, ServerCallContext context)
        {
            await Task.Delay(300);

            return new GreetdeadlineResponse() { Result = "Hello " + request.Message };
        }
    }
}
