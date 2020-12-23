using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;

namespace DevelApp.Workflow.SchemaBuilder
{
    public class DataOwnerDefinitionJsonSchema: AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> wfProps = new List<IJSBPart>();
            wfProps.Add(new JSBString("Name", "Returns the DataOwner unique name", maxLength: 100, isRequired: true));
            wfProps.Add(new JSBString("Version", "Returns the version of the DataOwner", isRequired: true));
            wfProps.Add(new JSBArray("ModuleDefinitions", "Returns the embedded module definitions", items: new List<IJSBPart>() {
             new JSBRef("ModuleDefinition", "Definition of an module", iriReference: new Uri("./ModuleDefinition.schema.json", UriKind.Relative))}, isRequired: true));
            JSBObject objectPart = new JSBObject("DataOwnerDefinition", "The easy definition of the schema of a dataOwner", props: wfProps);

            return new JSBSchema("DataOwnerDefinition", Description, topPart: objectPart);
        }

        public override string Description
        {
            get
            {
                return "The easy definition of the schema of a dataOwner";
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
