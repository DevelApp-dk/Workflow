using Akka.Actor;
using DevelApp.Utility.Model;
using System.Collections.ObjectModel;

namespace DevelApp.Workflow.Messages
{
    public class ListAllSagasMessage
    {
    }
    public class ListAllSagasSucceededMessage
    {
        public ListAllSagasSucceededMessage(ReadOnlyCollection<(KeyString, IActorRef)> sagas)
        {
            Sagas = sagas;
        }

        public ReadOnlyCollection<(KeyString, IActorRef)> Sagas { get; }
    }
}
