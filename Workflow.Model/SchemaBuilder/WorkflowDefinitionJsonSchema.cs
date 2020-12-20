using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.SchemaBuilder
{
    public class WorkflowDefinitionJsonSchema : AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> nProps = new List<IJSBPart>();
            nProps.Add(new JSBString("NodeKey", "NodeKey of the node", maxLength: 100));
            nProps.Add(new JSBString("Description", "Description of the node to make it more readable and understandable"));
            nProps.Add(new JSBString("BehaviorKey", "Key of the behavior to use for lookup", maxLength: 100));
            nProps.Add(new JSBString("BehaviorVersion", "Version of the behavior"));
            nProps.Add(new JSBString("BehaviorConfiguration", "Configuration of the instance of behavior"));
            nProps.Add(new JSBString("DataJsonSchemaModuleKey", "ModuleKey for looking up jsonschema for data", maxLength: 100));
            nProps.Add(new JSBString("DataJsonSchemaKey", "Key for looking up jsonschema for data", maxLength: 100));

            List<IJSBPart> eProps = new List<IJSBPart>();
            eProps.Add(new JSBString("Description", "Description of the edge"));
            eProps.Add(new JSBString("FromNodeKey", "The nodekey of the node that the edge comes from", maxLength: 100, isRequired: true));
            eProps.Add(new JSBString("ToNodeKey", "The nodekey of the node that the edge goes to", maxLength: 100, isRequired: true));

            List<IJSBPart> wfProps = new List<IJSBPart>();
            wfProps.Add(new JSBString("Name", "The version number of the workflow", maxLength: 100, isRequired: true));
            wfProps.Add(new JSBString("Version", "The version number of the workflow", isRequired: true));
            wfProps.Add(new JSBArray("Nodes", "Nodes of the workflow", items: new List<IJSBPart>() { 
             new JSBObject("Node", "Definition of an node", nProps)}, isRequired: true));
            wfProps.Add(new JSBArray("Edges", "Edges of the workflow", items: new List<IJSBPart>() {
             new JSBObject("Edge", "Definition of an edge", eProps)}, isRequired: true));
            JSBObject objectPart = new JSBObject("WorkflowDefinition", "The easy definition of the schema of a workflow", props: wfProps);

            return new JSBSchema("WorkflowDefinition", Description, topPart: objectPart);
        }


        public override string Description
        {
            get
            {
                return "The easy definition of the schema of a workflow";
            }
        }

        public override NamespaceString Module
        {
            get
            {
                return "Default";
            }
        }
    }
}
