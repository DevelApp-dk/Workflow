using Akka.Actor;
using DevelApp.Workflow.Core.Messages;
using Serilog;
using System;
using System.Threading;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("Console_.log", rollingInterval: RollingInterval.Day)
                            .CreateLogger(); 
            
            Log.Debug("Minor test");

            Console.WriteLine("Starting test");
            RunTest();
            Console.WriteLine("Ending test. Press any key to continue though only [Enter] works");
            Console.ReadLine();
        }

        private static void RunTest()
        {
            // Setup the actor system
            ActorSystem system = ActorSystem.Create("MySystem");
            
            Log.Debug("Start action");
            // Setup an actor that will handle deadletter type messages
            var persistenceTestActorProps = Props.Create(() => new PersistenceTestActor());
            var persistenceTestActorRef = system.ActorOf(persistenceTestActorProps, "PersistenceTestActor");


            //Wait here until system is up
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("Finished Sleeping. System up");


            persistenceTestActorRef.Tell(new WorkflowMessage("My dummy message", "We have som string content here"));





            //Wait here until tests are finished
            Thread.Sleep(TimeSpan.FromMinutes(3));
            Console.WriteLine("Finished Waiting for actor system");
        }
    }
}
