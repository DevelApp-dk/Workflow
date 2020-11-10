using DevelApp.JsonSchemaBuilder;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.SchemaBuilder
{
    public class WorkflowDefinitionJsonSchema : AbstractJsonSchema
    {
        public WorkflowDefinitionJsonSchema()
        {
            var nodesSchema = Array("Nodes", "Nodes of the workflow")
                .Item(Object("Node", "Definition of an node")
                    .Property("nodeKey", String("NodeKey", "NodeKey of the node")
                        .MaxLength(100))
                    .Property("description", String("Description", "Description of the node to make it more readable and understandable"))
                    .Property("behaviorKey", String("BehaviorKey", "Key of the behavior to use for lookup")
                        .MaxLength(100))
                    .Property("behaviorVersion", Integer("BehaviorVersion", "Version of the behavior"))
                    .Property("behaviorConfiguration", Integer("BehaviorConfiguration", "Configuration of the instance of behavior"))
                )
                ;

            var edgesSchema = Array("Edges", "Edges of the workflow")
                .Item(Object("Edge", "Definition of an edge")
                    .Property("description", String("Description", "Description of the edge"))
                    .Property("fromNodeKey", String("FromNodeKey", "The nodekey of the node that the edge comes from")
                        .MaxLength(100))
                    .Property("toNodeKey", String("ToNodeKey", "The nodekey of the node that the edge goes to")
                        .MaxLength(100))
                    .Required("fromNodeKey", "toNodeKey")
                    )
                ;

            var workflowSchema = Object("Workflow", "The easy definition of the schema of a workflow", true, true)
                .Property("name", String("Name", "The version number of the workflow")
                    .MaxLength(100)
                )
                .Property("version", Integer("Version", "The version number of the workflow"))
                .Property("nodes", nodesSchema)
                .Property("edges", edgesSchema)
                .Required("name", "version", "nodes", "edges")
                ;
            JsonSchema = workflowSchema;
        }

        public override string Description
        {
            get
            {
                return "The easy definition of the schema of a workflow";
            }
        }
    }
}
