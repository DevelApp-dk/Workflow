using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Core;

namespace Workflow.Model
{
    public class Saga:ISaga
    {
        /// <summary>
        /// Possibility to check if SagaStep data is valid before progressing
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public bool DataIsValid(ISagaStep sagaStep)
        {
            SchemaValidationResults results = CheckData(sagaStep);
            return results.IsValid;
        }

        /// <summary>
        /// Possibility to check if SagaStep DataJsonSchema is valid before progressing
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public bool DataJsonSchemaIsValid(ISagaStep sagaStep)
        {
            MetaSchemaValidationResults results = CheckDataJsonSchema(sagaStep);
            if (results == null)
            {
                return false;
            }
            return results.IsValid;
        }

        /// <summary>
        /// Possibility to check validation details of SagaStep data before progressing
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public SchemaValidationResults CheckData(ISagaStep sagaStep)
        {
            if (sagaStep.Data == null)
            {
                return new SchemaValidationResults() { IsValid = false, ErrorMessage = "SagaStep.Data is null" };
            }
            if (!DataJsonSchemaIsValid(sagaStep))
            {
                return new SchemaValidationResults() { IsValid = false, ErrorMessage = "SagaStep.DataJsonSchema is not valid" };
            }
            return sagaStep.DataJsonSchema.Validate(sagaStep.Data);
        }

        /// <summary>
        /// Possibility to check validation details of SagaStep DataJsonSchema before progressing. Returns null if DataJsonSchema is not found
        /// </summary>
        /// <param name="sagaStep"></param>
        /// <returns></returns>
        public MetaSchemaValidationResults CheckDataJsonSchema(ISagaStep sagaStep)
        {
            if (sagaStep.DataJsonSchema == null)
            {
                // Unfortuantely MetaSchemaValidationResults is not exposed to carry a message
                return null;
            }
            return sagaStep.DataJsonSchema.ValidateSchema();
        }

        /// <summary>
        /// Returns the Saga version
        /// </summary>
        /// <returns></returns>
        public string SagaVersion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the first SagaStep of the Saga
        /// </summary>
        /// <returns></returns>
        public ISagaStep FirstSagaStep
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the latest SagaStep of the Saga
        /// </summary>
        /// <returns></returns>
        public ISagaStep LatestSagaStep
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
