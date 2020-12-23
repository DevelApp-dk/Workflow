using Akka.Actor;
using Akka.Monitoring;
using DevelApp.Utility.Model;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.AbstractActors;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Messages;
using DevelApp.Workflow.Model;
using Manatee.Json;
using System;
using System.Collections.Generic;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Represents the individual sagas
    /// </summary>
    public class SagaActor : AbstractPersistedWorkflowActor<IWorkflowMessage, Saga>
    {
        private Model.Workflow _workflow;

        public SagaActor(Model.Workflow workflow, KeyString sagaKey)
        {
            SagaKey = sagaKey;
            _workflow = workflow;
        }

        protected Saga Saga { get; }

        public override string ActorId
        {
            get
            {
                return SagaKey;
            }
        }

        protected KeyString SagaKey { get; }

        protected override void WorkflowMessageHandler(IWorkflowMessage message)
        {
            switch (message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, Sender.Path);
                    if (!Sender.IsNobody() && !message.IsReply)
                    {
                        Sender.Tell((message as WorkflowMessage).GetWorkflowUnhandledMessage("Message Type Not Implemented", Self.Path));
                    }
                    break;
            }
        }

        /// <summary>
        /// Supervisory stategy for direct children with default handling
        /// </summary>
        /// <returns></returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    //Local
                    if (ex is ArithmeticException)
                    {
                        return Directive.Resume;
                    }

                    //Fallback to Default Stategy if not handled
                    return Akka.Actor.SupervisorStrategy.DefaultStrategy.Decider.Decide(ex);
                });
        }

        protected override void DoLastActionsAfterRecover()
        {
            //Pickup where last message died
            throw new NotImplementedException();
        }

        protected override void RecoverPersistedWorkflowDataHandler(IWorkflowMessage dataItem)
        {
            throw new NotImplementedException();
        }

        protected override void GroupFinishedMessageHandler(GroupFinishedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
