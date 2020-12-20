using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.SchemaBuilder
{
    public class ModuleDefinitionJsonSchema: AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> wfProps = new List<IJSBPart>();
            wfProps.Add(new JSBString("Name", "Returns the Module unique name", maxLength: 100, isRequired: true));
            wfProps.Add(new JSBString("Version", "Returns the version of the Module", isRequired: true));
            wfProps.Add(new JSBArray("WorkflowDefinitions", "Returns the embedded workflow definitions", items: new List<IJSBPart>() {
             new JSBRef("WorkflowDefinition", "Definition of an workflow", iriReference: new Uri("./WorkflowDefinition.schema.json", UriKind.Relative))}, isRequired: true));
            JSBObject objectPart = new JSBObject("ModuleDefinition", "The easy definition of the schema of a module", props: wfProps);

            return new JSBSchema("ModuleDefinition", Description, topPart: objectPart);
        }

        public override string Description
        {
            get
            {
                return "The easy definition of the schema of a module";
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
