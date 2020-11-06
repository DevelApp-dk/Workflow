using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DevelApp.Workflow.Actors
{
    public class DeadletterActor:ReceiveActor
    {
        private readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        public DeadletterActor(int actorInstance = 1)
        {
            ActorInstance = actorInstance;

            //Commands (like Receive)
            Receive<DeadLetter>(dl => {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received deadletter {1}", ActorId, dl.Message.ToString());
                DeadLetterMessageHandler(dl);
            });
        }

        #region Technical

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        private string ActorId
        {
            get
            {
                return GetType().Name.Replace("Actor", "") + $"_{ActorVersion}_{ActorInstance}";
            }
        }

        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        private int ActorVersion
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        private int ActorInstance { get; }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
        }
        #endregion

        /// <summary>
        /// Handles DeadLetters for workflow
        /// </summary>
        /// <param name="dl"></param>
        private void DeadLetterMessageHandler(DeadLetter dl)
        {
            Console.WriteLine($"DeadLetter captured: {dl.Message}, sender: {dl.Sender}, recipient: {dl.Recipient}");
            ReadOnlyCollection<(string Name, ActorPath ActorPath)> recipientList = DetermineRecipient(dl.Recipient);
            if (recipientList.Count == 0)
            {
                Console.WriteLine($"{GetType().Name} deadletter has odd empty actorPath so ignoring it");
            }
            else if (recipientList.Count > 0 && recipientList[0].ActorPath.Parent.Equals("system"))
            {
                Console.WriteLine($"{GetType().Name} deadletter is a system deadletter so ignoring it");
            }
            else
            {
                Context.ActorSelection(recipientList[0].ActorPath).Tell(new DeadletterHandlingMessage(recipientList, dl.Message), ActorRefs.NoSender);
            }
        }

        private ReadOnlyCollection<(string Name, ActorPath ActorPath)> DetermineRecipient(IActorRef recipient)
        {
            List<(string Name, ActorPath ActorPath)> recipientList = new List<(string Name, ActorPath ActorPath)>();
            ActorPath actorPath = recipient.Path;
            // Strip all until user or system
            while (actorPath.Parent != null && !actorPath.Parent.Name.Equals("user") && !actorPath.Parent.Name.Equals("system"))
            {
                //Set actorPath to parent if not like root
                actorPath = actorPath.Parent;
                recipientList.Add((actorPath.Name, actorPath));
            }
            recipientList.Reverse();
            return recipientList.AsReadOnly();
        }
    }
}
