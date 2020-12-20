using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.Workflow.Model.SchemaBuilder
{
    public class TranslationJsonSchema : AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> tProps = new List<IJSBPart>();
            tProps.Add(new JSBString("Key", "Lookup key for translation", isRequired: true));
            tProps.Add(new JSBString("Translation", "Returns the translation"));
            JSBObject objectPart = new JSBObject("Translation", Description, props: tProps);

            return new JSBSchema("Translation", Description, topPart: objectPart);
        }

        public override string Description
        {
            get
            {
                return "The definition of a translation";
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
