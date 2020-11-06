using System;
using System.Collections.Generic;
using System.Text;

namespace JsonSchemaBuilder
{
    public class NoValidationJsonSchema : JsonSchemaBuilder
    {
        public NoValidationJsonSchema()
        {
            JsonSchema = Manatee.Json.Schema.JsonSchema.Empty;
        }

        public override string Description
        {
            get
            {
                return "Represents an empty schema with disabled validation";
            }
        }
    }
}
