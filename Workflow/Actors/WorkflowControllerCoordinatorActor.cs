﻿using Akka.Actor;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    /// <summary>
    /// Handles the WorkflowControllerActor
    /// </summary>
    public class WorkflowControllerCoordinatorActor : AbstractWorkflowActor
    {
        protected override int ActorVersion
        {
            get
            {
                return 1;
            }
        }

        protected override void WorkflowMessageHandler(JsonValue message)
        {
            throw new NotImplementedException();
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
