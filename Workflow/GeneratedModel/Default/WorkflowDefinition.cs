using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ardalis.SmartEnum;
using System.Collections.Generic;
using System.Net.Mail;

namespace Default
{
    /// <summary>
    /// WorkflowDefinition: The easy definition of the schema of a workflow
    /// </summary>
    public partial class WorkflowDefinition
    {
        /// <summary>
        /// Name: The version number of the workflow
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// Version: The version number of the workflow
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }


        /// <summary>
        /// Node: Definition of an node
        /// </summary>
        public partial class Node
        {
            /// <summary>
            /// NodeKey: NodeKey of the node
            /// </summary>
            [JsonProperty("nodeKey")]
            public string NodeKey { get; set; }


            /// <summary>
            /// Description: Description of the node to make it more readable and understandable
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }


            /// <summary>
            /// BehaviorModuleKey: ModuleKey of the behavior to use for lookup
            /// </summary>
            [JsonProperty("behaviorModuleKey")]
            public string BehaviorModuleKey { get; set; }


            /// <summary>
            /// BehaviorKey: Key of the behavior to use for lookup
            /// </summary>
            [JsonProperty("behaviorKey")]
            public string BehaviorKey { get; set; }


            /// <summary>
            /// BehaviorVersion: Version of the behavior
            /// </summary>
            [JsonProperty("behaviorVersion")]
            public string BehaviorVersion { get; set; }


            /// <summary>
            /// BehaviorConfiguration: Configuration of the instance of behavior
            /// </summary>
            [JsonProperty("behaviorConfiguration")]
            public string BehaviorConfiguration { get; set; }


            /// <summary>
            /// DataJsonSchemaModuleKey: ModuleKey for looking up jsonschema for data
            /// </summary>
            [JsonProperty("dataJsonSchemaModuleKey")]
            public string DataJsonSchemaModuleKey { get; set; }


            /// <summary>
            /// DataJsonSchemaKey: Key for looking up jsonschema for data
            /// </summary>
            [JsonProperty("dataJsonSchemaKey")]
            public string DataJsonSchemaKey { get; set; }


            /// <summary>
            /// DataJsonSchemaVersion: Version of the jsonschema for data
            /// </summary>
            [JsonProperty("dataJsonSchemaVersion")]
            public string DataJsonSchemaVersion { get; set; }


        }
        /// <summary>
        /// Nodes: Nodes of the workflow
        /// </summary>
        [JsonProperty("nodes")]
        public List<Node> Nodes { get; set; } = new List<Node>();


        /// <summary>
        /// Edge: Definition of an edge
        /// </summary>
        public partial class Edge
        {
            /// <summary>
            /// Description: Description of the edge
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }


            /// <summary>
            /// FromNodeKey: The nodekey of the node that the edge comes from
            /// </summary>
            [JsonProperty("fromNodeKey")]
            public string FromNodeKey { get; set; }


            /// <summary>
            /// ToNodeKey: The nodekey of the node that the edge goes to
            /// </summary>
            [JsonProperty("toNodeKey")]
            public string ToNodeKey { get; set; }


        }
        /// <summary>
        /// Edges: Edges of the workflow
        /// </summary>
        [JsonProperty("edges")]
        public List<Edge> Edges { get; set; } = new List<Edge>();


    }
}
