﻿using Akka.Actor;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    ///// <summary>
    ///// Parent of the DataServiceControllerActor and several DataServiceWebhookActor
    ///// </summary>
    //public class DataServiceCoordinatorActor : AbstractPersistedWorkflowActor<string>
    //{
    //    protected override void RecoverPersistedWorkflowDataHandler(string data)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void RecoverPersistedSnapshotWorkflowDataHandler(string data)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void WorkflowMessageHandler(WorkflowMessage message)
    //    {
    //        switch (message.MessageTypeName)
    //        {
    //            default:
    //                Logger.Warning("{0} Did not handle received message [{1}] from [{2}]", ActorId, message.MessageTypeName, message.OriginalSender);
    //                Sender.Tell(new WorkflowUnhandledMessage(message, Self.Path));
    //                break;
    //        }
    //    }

    //    /// <summary>
    //    /// Supervisory stategy for direct children with default handling
    //    /// </summary>
    //    /// <returns></returns>
    //    protected override SupervisorStrategy SupervisorStrategy()
    //    {
    //        return new OneForOneStrategy(
    //            maxNrOfRetries: 10,
    //            withinTimeRange: TimeSpan.FromMinutes(1),
    //            localOnlyDecider: ex =>
    //            {
    //                //Local
    //                if (ex is ArithmeticException)
    //                {
    //                    return Directive.Resume;
    //                }

    //                //Fallback to Default Stategy if not handled
    //                return Akka.Actor.SupervisorStrategy.DefaultStrategy.Decider.Decide(ex);
    //            });
    //    }
    //}
}
