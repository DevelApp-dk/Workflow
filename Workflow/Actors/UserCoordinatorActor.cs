﻿using Akka.Actor;
using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.AbstractActors;
using DevelApp.Workflow.Core.Messages;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    public class UserCoordinatorActor : AbstractWorkflowActor
    {
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
    }
}
