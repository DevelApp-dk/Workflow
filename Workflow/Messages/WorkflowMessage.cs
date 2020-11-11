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
    public class WorkflowMessage
    {
        public WorkflowMessage(string messageTypeName, JsonValue data, IActorRef originalSender = default)
        {
            MessageTypeName = messageTypeName;
            Data = data;
            if (originalSender != default)
            {
                OriginalSender = originalSender;
            }
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
        /// Dynamic data for Message
        /// </summary>
        public JsonValue Data { get; }

        /// <summary>
        /// Can contain the original sender of the message
        /// </summary>
        public IActorRef OriginalSender { get; }
    }
}
