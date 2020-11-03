using Akka.Actor;
using Akka.Persistence;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    public abstract class AbstractWorkflowActor: ReceiveActor
    {

        public AbstractWorkflowActor()
        {
            //Commands (like Receive)
            Receive<JsonValue>(message => { WorkflowMessageHandler(message); });
        }
        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract int Actor_Version { get; }

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(JsonValue message);
    }
}
