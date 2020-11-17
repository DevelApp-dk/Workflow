using DevelApp.Workflow.Core;
using DevelApp.Workflow.Core.Exceptions;
using DevelApp.Workflow.Core.Model;
using DoubleLinkedDirectedGraph;
using Manatee.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public class Workflow
    {
        private DoubleLinkedDirectedGraph<NodeData, EdgeData> _internalWorkflow = new DoubleLinkedDirectedGraph<NodeData, EdgeData>();
        private ISagaStepBehaviorFactory _sagaStepBehaviorFactory;

        /// <summary>
        /// Builds workflow from workflow template
        /// </summary>
        /// <param name="workflow"></param>
        public Workflow(WorkflowDefinition workflowDefinition, ISagaStepBehaviorFactory sagaStepBehaviorFactory)
        {
            Name = workflowDefinition.Name;
            Version = workflowDefinition.Version;
            _sagaStepBehaviorFactory = sagaStepBehaviorFactory;

            //Add all edges starting from "START" with their nodes
            foreach (WorkflowDefinition.Edge edge in workflowDefinition.Edges.Where(e=>e.FromNodeKey.Equals(DoubleLinkedDirectedGraph<NodeData, EdgeData>.START_NODE_KEY)))
            {
                WorkflowDefinition.Node toNode = workflowDefinition.Nodes.Where(n => n.NodeKey.Equals(edge.ToNodeKey)).FirstOrDefault();
                CheckIfSagaStepBehaviorExists(toNode);
                _internalWorkflow.InsertFromStart(toNode.NodeKey, new NodeData(toNode.Description, toNode.BehaviorKey, toNode.BehaviorVersion, toNode.BehaviorConfiguration));
            }

            //Add all remaining edges with their nodes
            List<WorkflowDefinition.Edge> nonInsertedEdges = workflowDefinition.Edges.Where(e => !e.FromNodeKey.Equals(DoubleLinkedDirectedGraph<NodeData, EdgeData>.START_NODE_KEY)).ToList();
            while(nonInsertedEdges.Count > 0)
            {
                //Select first that have an included nodekey to avoid problems
                WorkflowDefinition.Edge edge = nonInsertedEdges.Where(f => _internalWorkflow.AlreadyAddedNodeKeys.Contains(f.FromNodeKey)).First();
                WorkflowDefinition.Node toNode = workflowDefinition.Nodes.Where(n => n.NodeKey.Equals(edge.ToNodeKey)).FirstOrDefault();
                CheckIfSagaStepBehaviorExists(toNode);
                _internalWorkflow.Insert(edge.FromNodeKey, toNode.NodeKey, edge.Description, new NodeData(toNode.Description, toNode.BehaviorKey, toNode.BehaviorVersion, toNode.BehaviorConfiguration));

                nonInsertedEdges.Remove(edge);
            }

            _internalWorkflow.FinishGraph();
        }

        private void CheckIfSagaStepBehaviorExists(WorkflowDefinition.Node toNode)
        {
            ISagaStepBehavior sagaStepBehavior = _sagaStepBehaviorFactory.GetSagaStepBehavior(toNode.BehaviorKey, toNode.BehaviorVersion, toNode.BehaviorConfiguration);
            if(sagaStepBehavior == null)
            {
                throw new WorkflowStartupException($"Workflow {Name}.{Version} cannot find SagaStepBehavior {toNode.BehaviorKey}.{toNode.BehaviorVersion}");
            }
        }

        public ISagaStepBehavior GetSagaStepBehaviorForNode(DoubleLinkedDirectedGraph<NodeData, EdgeData>.Node node)
        {
            ISagaStepBehavior sagaStepBehavior = _sagaStepBehaviorFactory.GetSagaStepBehavior(node.NodeData.BehaviorKey, node.NodeData.BehaviorVersion, node.NodeData.BehaviorConfiguration);
            if (sagaStepBehavior == null)
            {
                throw new WorkflowRuntimeException($"Workflow {Name}.{Version} cannot find SagaStepBehavior {node.NodeData.BehaviorKey}.{node.NodeData.BehaviorVersion}");
            }
            else
            {
                return sagaStepBehavior;
            }
        }

        /// <summary>
        /// Name of the Workflow
        /// </summary>
        public KeyString Name { get; }

        /// <summary>
        /// Version number of the workflow
        /// </summary>
        public VersionNumber Version { get; }

        /// <summary>
        /// Returns all the start nodes
        /// </summary>
        public IEnumerable<DoubleLinkedDirectedGraph<NodeData, EdgeData>.Node> Start
        {
            get
            {
                return _internalWorkflow.Start;
            }
        }

        /// <summary>
        /// Returns all the end nodes
        /// </summary>
        public IEnumerable<DoubleLinkedDirectedGraph<NodeData, EdgeData>.Node> End
        {
            get
            {
                return _internalWorkflow.End;
            }
        }

        /// <summary>
        /// Node specific data
        /// </summary>
        public class NodeData
        { 
            public NodeData(string description, KeyString behaviorKey, VersionNumber behaviorVersion, JsonValue behaviorConfiguration)
            {
                Description = description;
                BehaviorKey = behaviorKey;
                BehaviorVersion = behaviorVersion;
                BehaviorConfiguration = behaviorConfiguration;
            }

            /// <summary>
            /// Description of the node to make it human readable and understandable
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// The behavior of the sagaKey
            /// </summary>
            public KeyString BehaviorKey { get; }

            /// <summary>
            /// The version of the behavior
            /// </summary>
            public VersionNumber BehaviorVersion { get; }

            /// <summary>
            /// The configuration of the behavior
            /// </summary>
            public JsonValue BehaviorConfiguration { get; }
        }

        /// <summary>
        /// Edge specific data which is a dummy here
        /// </summary>
        public class EdgeData
        {
            public EdgeData()
            {
            }
        }
    }
}
