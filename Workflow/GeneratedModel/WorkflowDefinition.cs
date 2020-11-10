using System;
using System.Collections.Generic;
using System.Text;
using Manatee.Json;
using Newtonsoft.Json;

namespace DevelApp.Workflow.Model
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
		public long Version { get; set; }

		/// <summary>
		/// Node: Definition of an node
		/// </summary>
		public sealed partial class Node
		{
			/// <summary>
			/// NodeKey: NodeKey of the node
			/// </summary>
			[JsonProperty("nodeKey")]
			public string NodeKey { get; set; }

			/// <summary>
			/// Description: Description of the node to make it more readable
			///  and understandable
			/// </summary>
			[JsonProperty("description")]
			public string Description { get; set; }

			/// <summary>
			/// BehaviorKey: Key of the behavior to use for lookup
			/// </summary>
			[JsonProperty("behaviorKey")]
			public string BehaviorKey { get; set; }

			/// <summary>
			/// BehaviorVersion: Version of the behavior
			/// </summary>
			[JsonProperty("behaviorVersion")]
			public long BehaviorVersion { get; set; }

			/// <summary>
			/// BehaviorConfiguration: Configuration of the instance of behavior
			/// </summary>
			[JsonProperty("behaviorConfiguration")]
			public JsonValue BehaviorConfiguration { get; set; }

		}


		/// <summary>
		/// Nodes: Nodes of the workflow
		/// </summary>
		[JsonProperty("nodes")]
		public List<Node> Nodes { get; set; } = new List<Node>();

		/// <summary>
		/// Edge: Definition of an edge
		/// </summary>
		public sealed partial class Edge
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
