using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Core;

namespace DevelApp.Workflow.Model
{
    public class SagaStep:ISagaStep
    {
        private Saga _saga;
        public SagaStep(Saga saga)
        {
            _saga = saga;
        }

        /// <summary>
        /// Returns the owning Saga
        /// </summary>
        /// <returns></returns>
        public ISaga Saga
        {
            get
            {
                return _saga;
            }
        }

        /// <summary>
        /// Returns all the previous SagaSteps in the order previous first
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISagaStep> AllPreviousSagaSteps
        {
            get
            {
                ISagaStep sagaStep = this;
                while (sagaStep != null)
                {
                    yield return sagaStep;
                    sagaStep = sagaStep.PreviousSagaStep;
                }
            }
        }

        /// <summary>
        /// Returns the previous SagaStep
        /// </summary>
        /// <returns></returns>
        public ISagaStep PreviousSagaStep
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the data of the current SagaStep
        /// </summary>
        /// <returns></returns>
        public JsonValue Data { get; set; }

        /// <summary>
        /// The JsonSchema for the data of the current SagaStep
        /// </summary>
        public JsonSchema DataJsonSchema { get; set; }

        /// <summary>
        /// Returns the configuration for the SagaStep
        /// </summary>
        /// <returns></returns>
        public JsonValue Configuration
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
