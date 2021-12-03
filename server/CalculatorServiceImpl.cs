using Calculator;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace server
{
    internal class CalculatorServiceImpl : CalculatorServiceBase
    {
        public override Task<OperationResponse> Sum(OperationRequest request, ServerCallContext context)
        {
            int result = request.Number1 + request.Number2;
            return Task.FromResult(new OperationResponse() { Result = result });
        }
    }
}
