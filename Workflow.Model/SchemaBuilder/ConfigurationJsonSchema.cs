using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model.SchemaBuilder
{
    public class ConfigurationJsonSchema : AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> tProps = new List<IJSBPart>();
            tProps.Add(new JSBString("Key", "Lookup key for configuration", isRequired: true));
            tProps.Add(new JSBString("Configuration", "Returns the configuration"));
            JSBObject objectPart = new JSBObject("Configuration", Description, props: tProps);

            return new JSBSchema("Configuration", Description, topPart: objectPart);
        }

        public override string Description
        {
            get
            {
                return "The definition of a configuration";
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
