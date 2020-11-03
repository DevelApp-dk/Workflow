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

        public AbstractPersistedWorkflowActor(int snapshotPerVersion = 1)
        {
            _snapshotPerVersion = snapshotPerVersion;

            //Recover
            Recover<JsonValue>(data => { RecoverPersistedWorkflowDataHandler(data); });

            //Commands (like Receive)
            Command<JsonValue>(message => { WorkflowMessageHandler(message); });

            Command<SaveSnapshotSuccess>(success => {
                // soft-delete the journal up until the sequence # at
                // which the snapshot was taken
                DeleteMessages(success.Metadata.SequenceNr);
            });

            //TODO Handle snapshot failue
            //Command<SaveSnapshotFailure>(failure => {
            //    failure.Cause;
            //});
        }

        /// <summary>
        /// Returns the actor version in positive number
        /// </summary>
        protected abstract int Actor_Version { get; }

        /// <summary>
        /// Returns the persistant name as default. Override on 
        /// </summary>
        public override string PersistenceId
        {
            get
            {
                return GetType().Name.Replace("Actor", "") + $"_{Actor_Version}";
            }
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
        /// <param name="message"></param>
        protected abstract void RecoverPersistedWorkflowDataHandler(JsonValue data);

        /// <summary>
        /// Handle incoming Workflow Messages
        /// </summary>
        /// <param name="message"></param>
        protected abstract void WorkflowMessageHandler(JsonValue message);
    }
}
