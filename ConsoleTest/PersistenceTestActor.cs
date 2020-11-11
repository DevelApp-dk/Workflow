using DevelApp.Workflow.Actors;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    public class PersistenceTestActor : ConsoleTest.AbstractPersistedWorkflowActor<JsonValue>
    {
        private int recovercalls = 0;
        public PersistenceTestActor() : base(snapshotPerVersion: 100)
        { 
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(JsonValue data)
        {
            recovercalls += 1;
            Console.WriteLine($"Recover {recovercalls} times called with [{data.String}]");
        }

        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            Console.WriteLine($"Message {message.MessageTypeName} received");
            PersistWorkflowData(message.Data);
        }

        protected override void PreStart()
        {
            base.PreStart();
            Console.WriteLine($"{ActorId} is starting");
        }

        protected override void PostStop()
        {
            Console.WriteLine($"{ActorId} is stopping");
            base.PostStop();
        }
    }
}
