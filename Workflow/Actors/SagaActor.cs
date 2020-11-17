using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using DevelApp.Workflow.Model;
using Manatee.Json;
using System;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Represents the individual sagas
    /// </summary>
    public class SagaActor : AbstractPersistedWorkflowActor<Saga, Saga>
    {
        private Model.Workflow _workflow;

        public SagaActor(Model.Workflow workflow, KeyString sagaKey) :base(snapshotPerVersion: 1)
        {
            SagaKey = sagaKey;
            _workflow = workflow;
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override string ActorId
        {
            get
            {
                return SagaKey;
            }
        }

        protected KeyString SagaKey { get; }

        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            switch(message.MessageTypeName)
            {
                default:
                    Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, message.OriginalSender);
                    Sender.Tell(new WorkflowUnhandledMessage(message, Self.Path));
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
            //TODO see if actions needs to continue based on Saga
            throw new NotImplementedException();
        }

        protected override void RecoverPersistedWorkflowDataHandler(Saga dataItem)
        {
            Logger.Warning("{0} Did received message RecoverPersistedWorkflowDataHandler but should not as only snapshot should be used");
        }
    }
}
