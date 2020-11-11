using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using DevelApp.Workflow.Core.Model;
using DevelApp.Workflow.Messages;
using Manatee.Json;

namespace DevelApp.Workflow.Actors
{
    public abstract class AbstractPersistedWorkflowActor<T> : ReceivePersistentActor where T:class
    {
        private int _snapshotPerVersion;
        private int _persistsSinceLastSnapshot;
        protected readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        public AbstractPersistedWorkflowActor(int actorInstance = 1, int snapshotPerVersion = 1)
        {
            ActorInstance = actorInstance;

            _snapshotPerVersion = snapshotPerVersion;

            //Recover
            Recover<T>(data => {
                Logger.Debug("{0} recovered data {1}", ActorId, data.ToString());
                RecoverPersistedWorkflowDataHandler(data); 
            });

            Recover<SnapshotOffer>(offer => {
                Logger.Debug("{0} offered snapshot {1}", ActorId, offer.Snapshot.ToString());
                T data = offer.Snapshot as T;
                RecoverPersistedSnapShotWorkflowDataHandler(data);
            });

            //Commands (like Receive)
            Command<WorkflowMessage>(message => {
                Context.IncrementMessagesReceived();
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                WorkflowMessageHandler(message); 
            });

            Command<SaveSnapshotSuccess>(success => {
                Logger.Debug("SaveSnapshot succeeded for {0} so deleting messages until this snapshot", PersistenceId);
                // soft-delete the journal up until the sequence # at
                // which the snapshot was taken
                DeleteMessages(success.Metadata.SequenceNr);
            });

            //Handle snapshot failue
            Command<SaveSnapshotFailure>(failure =>
            {
                Logger.Debug(failure.Cause, "SaveSnapshot Failed for {0}", PersistenceId);
            });

            //Handle snapshot failue
            Command<DeleteMessagesFailure>(failure =>
            {
                Logger.Debug(failure.Cause, "DeleteMessages Failed for {0}", PersistenceId);
            });

            //Handle deleted messages success
            Command<DeleteMessagesSuccess>(message =>
            {
                //Do nothing
            }
            );

            Command<DeadletterHandlingMessage>(message => {
                Context.IncrementCounter(nameof(DeadletterHandlingMessage));
                Logger.Debug("{0} received message {1}", ActorId, message.ToString());
                DeadletterHandlingMessageHandler(message);
            });
        }

        /// <summary>
        /// Returns the unique actor id
        /// </summary>
        protected virtual string ActorId
        {
            get
            {
                return GetType().Name.Replace("Actor", "") + $"_{ActorVersion}_{ActorInstance}";
            }
        }

        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract VersionNumber ActorVersion { get; }

        /// <summary>
        /// Returns the persistant name as default. Override on 
        /// </summary>
        public override string PersistenceId
        {
            get
            {
                return ActorId;
            }
        }

        /// <summary>
        /// Returns the actor instance
        /// </summary>
        protected int ActorInstance { get; }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PreStart()
        {
            base.PreStart();
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
            base.PostStop();
        }

        /// <summary>
        /// Persists data in versions until snapshotPerVersion
        /// </summary>
        /// <param name="data"></param>
        protected void PersistWorkflowData(T data)
        {
            if (_snapshotPerVersion <= 1)
            {
                SaveSnapshot(data);
            }
            else
            {
                Persist(data, s =>
                {
                    if (++_persistsSinceLastSnapshot % _snapshotPerVersion == 0)
                    {
                        //time to save a snapshot
                        SaveSnapshot(data);
                    }
                });
            }
        }
    

        /// <summary>
        /// Used for recovering from crash
        /// </summary>
        /// <param name="data"></param>
        protected abstract void RecoverPersistedWorkflowDataHandler(T data);

        /// <summary>
        /// Snapshot is offered to start Recover process
        /// </summary>
        /// <param name="data"></param>
        protected abstract void RecoverPersistedSnapShotWorkflowDataHandler(T data);

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(WorkflowMessage message);

        /// <summary>
        /// Handles DeadletterHandlingMessage. Default is to log and ignore
        /// </summary>
        /// <param name="message"></param>
        protected virtual void DeadletterHandlingMessageHandler(DeadletterHandlingMessage message)
        {
            Logger.Debug("{0} received message {1}", ActorId, message.ToString());
        }
    }
}
