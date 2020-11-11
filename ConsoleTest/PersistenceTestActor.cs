using DevelApp.Workflow.Actors;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    public class PersistenceTestActor : ConsoleTest.AbstractPersistedWorkflowActor<string>
    {
        private int recovercalls = 0;
        public PersistenceTestActor() : base(actorInstance: 1, snapshotPerVersion: 1)
        { 
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(string data)
        {
            recovercalls += 1;
            Logger.Debug($"Recover {recovercalls} times called with [{data}]");
        }

        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            Logger.Debug($"Message {message.MessageTypeName} received");
            PersistWorkflowData(message.MessageTypeName);
        }

        protected override void PreStart()
        {
            base.PreStart();
            Logger.Debug($"{ActorId} is starting");
        }

        protected override void PostStop()
        {
            Logger.Debug($"{ActorId} is stopping");
            base.PostStop();
        }
    }
}
