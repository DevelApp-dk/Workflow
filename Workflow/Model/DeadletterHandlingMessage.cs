using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public class DeadletterHandlingMessage
    {
        public DeadletterHandlingMessage(ReadOnlyCollection<(string Name, ActorPath ActorPath)> recipientList, object message)
        {
            RecipientList = recipientList;
            Message = message;
        }

        public ReadOnlyCollection<(string Name, ActorPath ActorPath)> RecipientList { get; }
        public object Message { get; }
    }
}
