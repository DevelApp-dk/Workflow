using Akka.Actor;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Messages
{
    /// <summary>
    /// Used as envelope for many dynamic message types used in Workflow
    /// </summary>
    public class WorkflowUnhandledMessage
    {
        public WorkflowUnhandledMessage(WorkflowMessage workflowMessage, ActorPath replyingReceiver)
        {
            MessageTypeName = workflowMessage.MessageTypeName;
            Data = workflowMessage.Data;
            ReplyingReceiver = replyingReceiver;
            OriginalMessageCreationTime = workflowMessage.MessageCreationTime;
            MessageCreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// MessageTypeName is used to distinguish what the message is about
        /// </summary>
        public string MessageTypeName { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime MessageCreationTime { get; }

        /// <summary>
        /// Time of message creation in UTC time
        /// </summary>
        public DateTime OriginalMessageCreationTime { get; }

        /// <summary>
        /// Dynamic data for Message
        /// </summary>
        public JsonValue Data { get; }

        /// <summary>
        /// Can contain the original sender of the message
        /// </summary>
        public ActorPath ReplyingReceiver { get; }
    }
}
