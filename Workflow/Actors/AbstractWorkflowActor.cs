using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    public abstract class AbstractWorkflowActor: ReceiveActor
    {
        protected readonly ILoggingAdapter ActorLog = Logging.GetLogger(Context);

        public AbstractWorkflowActor(int actorInstance = 1)
        {
            ActorInstance = actorInstance;

            //Commands (like Receive)
            Receive<JsonValue>(message => {
                Context.IncrementMessagesReceived();
                ActorLog.Debug("{0} received message {1}", ActorId, message.ToString());
                WorkflowMessageHandler(message); 
            });
        }

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        protected virtual string ActorId
        {
            get
            {
                return GetType().Name.Replace("Actor", "") + $"_{ActorVersion}_{ActorInstance}";
            }
        }

        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract int ActorVersion { get; }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        protected int ActorInstance { get; }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
        }

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(JsonValue message);
    }
}
