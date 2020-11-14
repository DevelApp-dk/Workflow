using Akka.Actor;
using Akka.Monitoring;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Interfaces;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    /// <summary>
    /// Singleton parent of potentially multiple DataOwnerActor
    /// </summary>
    public class DataOwnerCoordinatorActor : AbstractPersistedWorkflowActor<IDataOwnerCRUDMessage, Dictionary<string, IDataOwnerCRUDMessage>>
    {
        public DataOwnerCoordinatorActor()
        {
            Command<IDataOwnerCRUDMessage>(message => {
                Context.IncrementMessagesReceived();
                DataOwnerCRUDMessageHandler(message);
            });
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void RecoverPersistedWorkflowDataHandler(IDataOwnerCRUDMessage data)
        {
            DataOwnerCRUDMessageHandler(data);
        }

        private void DataOwnerCRUDMessageHandler(IDataOwnerCRUDMessage message)
        {
            switch (message.CRUDMessageType)
            {
                case Model.CRUDMessageType.Create:
                    CreateDataOwnerMessage createDataOwnerMessage = message as CreateDataOwnerMessage;
                    //Create dataOwner from createDataOwnerMessage.DataOwnerDefinition and pass createDataOwnerMessage.DataOwnerDefinition.ModuleDefinitions to child
                    break;
                default:
                    //TODO delete
                    throw new WorkflowStartupException("CRUD Message Type Not Implemented");
            }
        }


        protected override void WorkflowMessageHandler(WorkflowMessage message)
        {
            switch (message.MessageTypeName)
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
            throw new NotImplementedException();
        }
    }
}
