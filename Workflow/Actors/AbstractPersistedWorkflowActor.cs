using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using Manatee.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Actors
{
    public abstract class AbstractPersistedWorkflowActor : ReceivePersistentActor
    {
        private int _snapshotPerVersion;
        private int _persistsSinceLastSnapshot;
        protected readonly ILoggingAdapter ActorLog = Logging.GetLogger(Context);

        public AbstractPersistedWorkflowActor(int actorInstance = 1, int snapshotPerVersion = 1)
        {
            ActorInstance = actorInstance;

            _snapshotPerVersion = snapshotPerVersion;

            //Recover
            Recover<JsonValue>(data => {
                ActorLog.Debug("{0} recovered data {1}", ActorId, data.ToString());
                RecoverPersistedWorkflowDataHandler(data); 
            });

            //Commands (like Receive)
            Command<JsonValue>(message => {
                Context.IncrementMessagesReceived();
                ActorLog.Debug("{0} received message {1}", ActorId, message.ToString());
                WorkflowMessageHandler(message); 
            });

            Command<SaveSnapshotSuccess>(success => {
                ActorLog.Debug("SaveSnapshot succeeded for {0} so deleting messages until this snapshot", PersistenceId);
                // soft-delete the journal up until the sequence # at
                // which the snapshot was taken
                DeleteMessages(success.Metadata.SequenceNr);
            });

            //Handle snapshot failue
            Command<SaveSnapshotFailure>(failure =>
            {
                ActorLog.Debug(failure.Cause, "SaveSnapshot Failed for {0}", PersistenceId);
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
        protected abstract int ActorVersion { get; }

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
            Context.IncrementActorCreated();
        }

        /// <summary>
        /// Increment Monitoring Actor Created
        /// </summary>
        protected override void PostStop()
        {
            Context.IncrementActorStopped();
        }

        /// <summary>
        /// Persists data in versions until snapshotPerVersion
        /// </summary>
        /// <param name="data"></param>
        protected void PersistWorkflowData(JsonValue data)
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
        protected abstract void RecoverPersistedWorkflowDataHandler(JsonValue data);

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(JsonValue message);
    }
}
