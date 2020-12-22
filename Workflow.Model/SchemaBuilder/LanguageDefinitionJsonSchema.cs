using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model.SchemaBuilder
{
    public class LanguageDefinitionJsonSchema : AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> tProps = new List<IJSBPart>();
            tProps.Add(new JSBString("Key", "Lookup key for language", isRequired: true));
            tProps.Add(new JSBString("Language", "Returns the language"));
            JSBObject objectPart = new JSBObject("LanguageDefinition", Description, props: tProps);

            return new JSBSchema("LanguageDefinition", Description, topPart: objectPart);
        }

        public override string Description
        {
            get
            {
                return "The definition of a language";
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
