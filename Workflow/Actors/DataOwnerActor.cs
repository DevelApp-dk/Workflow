using Akka.Actor;
using Akka.Monitoring;
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
    /// Parent of multiple ModuleActor
    /// </summary>
    public class DataOwnerActor : AbstractPersistedWorkflowActor<IModuleCRUDMessage, Dictionary<string, IModuleCRUDMessage>>
    {
        public DataOwnerActor()
        {
            Command<IModuleCRUDMessage>(message => {
                Context.IncrementMessagesReceived();
                PersistWorkflowData(message);
                ModuleCRUDMessageHandler(message);
            });
        }

        private void ModuleCRUDMessageHandler(IModuleCRUDMessage message)
        {
            throw new NotImplementedException();
        }

        protected override VersionNumber ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void DoLastActionsAfterRecover()
        {
            Logger.Debug("{0} Finished restoring", ActorId);
        }

        protected override void RecoverPersistedWorkflowDataHandler(IModuleCRUDMessage dataItem)
        {
            ModuleCRUDMessageHandler(dataItem);
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
    }
}
