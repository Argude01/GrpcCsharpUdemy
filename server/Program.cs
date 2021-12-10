﻿using Calculator;
using Greet;
using Greetdeadline;
using Grpc.Core;
using Sqrt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Program
    {
        const int Port = 50051;

        static void Main(string[] args)
        {
            Server server = null;
            try
            {
                var serverCert = File.ReadAllText("ssl/server.crt");
                var serverKey = File.ReadAllText("ssl/server.key");
                var keypair = new KeyCertificatePair(serverCert, serverKey);
                var cacert = File.ReadAllText("ssl/ca.crt");

                var credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, true);

                server = new Server()
                {
                    Services = { 
                        GreetingService.BindService( new GreetingServiceImpl() ),
                        SqrtService.BindService( new SqrtServiceImpl() ),
                        CalculatorService.BindService( new CalculatorServiceImpl() ),
                        GreetdeadlineService.BindService( new GreetdeadlineServiceImpl() )                    
                    },
                    Ports = { 
                        new ServerPort("localhost", Port, credentials) 
                    },
                    //Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine("The server is listening on the port : " + Port);
                Console.ReadKey();  
            }
            catch (IOException e)
            {
                Console.WriteLine("The server failed to start : " + e.Message);
                throw;
            }
            finally
            {
                if (server != null)
                    server.ShutdownAsync().Wait();
            }
        }
    }
}
