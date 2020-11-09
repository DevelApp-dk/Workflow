using DevelApp.Workflow.Core.Model;
using DoubleLinkedDirectedGraph;
using Manatee.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model
{
    public class Workflow
    {
        private DoubleLinkedDirectedGraph<NodeData, EdgeData> internalWorkflow = new DoubleLinkedDirectedGraph<NodeData, EdgeData>();

        /// <summary>
        /// Builds workflow from workflow template
        /// </summary>
        /// <param name="workflow"></param>
        public Workflow(JsonValue workflow)
        {
            //Get all nodes
            //Get all edges
            //Add all edges starting from "START" with their nodes
            //Add all remaining edges with their nodes

        }

        /// <summary>
        /// Returns all the start nodes
        /// </summary>
        public IEnumerable<DoubleLinkedDirectedGraph<NodeData, EdgeData>.Node> Start
        {
            get
            {
                return internalWorkflow.Start;
            }
        }

        /// <summary>
        /// Returns all the end nodes
        /// </summary>
        public IEnumerable<DoubleLinkedDirectedGraph<NodeData, EdgeData>.Node> End
        {
            get
            {
                return internalWorkflow.End;
            }
        }

        /// <summary>
        /// Node specific data
        /// </summary>
        public class NodeData
        { 
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
        /// Edge specific data
        /// </summary>
        public class EdgeData
        {
            /// <summary>
            /// 
            /// </summary>
            public string description { get; }
        }
    }
}
