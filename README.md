# Workflow
Workflow Orchestration using Quartz.Net as internal scheduler to make is easier to created uncoupled workflow with frontend, automation and integration

## Features

## Reasoning and definitions
When making selfservice applications, integrations and automations there is a high need to expose program flow for business users as application of business rules requires ability to reason about program flow without need for programming abilities for the business users.
Workflow is based on a mix of flow based programming and executable statecharts (orchistration of the individual workflows and the tasks in the workflow). 
* Flow based programming: In short flow based programming is a clear separation of control of flow and specific tasks in the flow making it easier to follow the flow and reason about the flow with non-software developers. Invented by J. Paul Morrison in early 1970s. 
* Statecharts: Statecharts is statemachines that support parallel and substate execution. Statecharts used is a subform of [SCXML](https://www.w3.org/TR/scxml/) which I call a sharable statechart where the specific actions are specified but not containing specific functions because that is programming handling more than flow and seems to muddle the ability to reason on the flow. The statecharts are used to execute the flow providing a "As Implemented" documentation of the program flow. Frontend definitions of workflows are delivered as XState defined statecharts.
* Workflow: Workflows are sometimes called orchistrations especially in Service Broker implementations. A Workflow composed of Steps and Transitions. The individual workflows can be triggered by a timed schedule or by interaction. The workflow defines the retetion period of the Saga data
* Step: Each step is specifying a specific handling based on the step properties and types. Each state uses a state action which is an object with one or more functions within defined by an interface to the statetype
  * Steptypes:
    * Automatic: Running without user interaction. Has three functions: [OnEntry], [Execute] and [OnExit].
    * User interaction: Running with user interaction. Has four backend functions: [OnEntry], [CreateOutputData], [ValidateInput] and [OnExit] and on the frontend step handles the step execution based on output data and input data. The format of the input and output data is defined based on Json Schema making it easy to autogenerate and validate both on backend and on frontend. After the [CreateOutput] function is run the backend suspends the execution and a message based on SagaStep is sent to the frontend. When a message is received from the frontend Json Schema validation is run on the message form and if passing the [ValidateInputData] is run to make the backend validation on the message content. If all validation is passed the state transition is done based on the Outcome decided in the frontend and a message is sendt to the frontend that transition for the next state is allowed. If one validation fails a message is sent to the user interface and the SagaStep is updated.
    * Subworkflow: Initiates one or more sub workflows. Has four functions: [OnEntry], [StartSubWorkflows], [MergeSubWorkflows] and [OnExit]
  * Step Action: Individual executable code parts based on step types. This is made with .Net code but it would also be possible to expand with other languages
    * [OnEntry] for guard functionality and preprocessing
    * [Execute] for main execution responsible for deciding Output
    * [CreateOutputData] for preparing the output to the user interface
    * [ValidateInputData] validates the message content based on the selected Output
    * [StartSubWorkflows] schedules or starts one or more workflows with start data and notes on the subflow saga which saga initiated the subflow to make  
    * [MergeSubWorkflows] is called when all the subflows have completed (either run to Output Complete or in Output Error) or when a timeout occured
    * [OnExit] for post processing typically Saga update
* Transition: A transition is a connection between two Steps. One Steps Output is connected to another Step
* Saga: A sage is normally a long running transaction. Here the responsibility is for handling the data of the workflow instance. It is comprised of a ordered list of SagaSteps. The last step shows the current Step of the Workflow instance.
* SagaStep: A SagaStep is used for storing each individual Step the workflow has gone through and the data conserning the step. This can be used for handling rollback on errors. Mainly SagaSteps document what happens in execution and the data used.

## Best practice
* If the workflow updates or deletes data first step should get the existing data for storing in the SagaStep for both supporting error rollback and verifying the update starts for the correct data point. As a workflow execution can take months this makes it useful to store the original for checking original data before the change so the transition is from a consistent state to another consistent state. 
  * One exapmle could be a workflow is responsible for inserting some money on a persons account. He has 4 DKK on his microcredit account when starting the workflow, 100 DKK has been deducted from his CreditCard, he got a receipt that he now should have 104 on his microcredit account and the workflow suspends for a week because of some interaction is needed or an error occured. After the week the microcredit account is at 2 DKK when the workflow resumes and the money needs to be written to his microcredit account. Because you have the original content of the microcredit account validation of the content would say that the microcredit account content is different than the start making an error not updateing the content to 104 DKK but goes for simple error handling which updates to 102 and notifies the customer of the correct change in microcredit account credit.
  * A customer uses a selfservice platform to change his email address from tester@outlook.com to testera@hotmail.com in which he makes an error (should have been tester@hotmail.com) so that the email validation is not possible as he is not receiving an email. He realises this after a week and calls support and the support changes his email to the correct tester@hotmail.com. An error in the validation workflow makes the workflow start again after the week has gone and trying to update his email from tester@outlook.com to testera@hotmail.com but validation catches that is is trying to update from tester@outlook.com but the current data is tester@hotmail.com making the update fail as it should.
  
## Limitations
* The specific implementations and decoupling makes it not a useful as a data-processing pipeline for high speed processing

## Benefits
* When errors occur it is faster to find and correct them improving the response time on errors and reducing a possible downtime
* It is easier for new developers to start as the statechart makes is easier to work with only one state at the time and the possible outcome (in flow based programming called a port) of that state specific function leading to a transition to another state. This is applicable on the frontend as well as the backend. This makes the functions of each state akin to Azure Functions and Amazon Lambdas.
* Decrease the effect of cowboy/spagetthi code as it would only affect the function of one state
* Decrease the amount of errors as program flow is exposed
* Increase the testability of the software
* Makes it easier to split execution either based on entire workflow or individual states and their functions. In microservices logic the split would be to have each individual state handling as a specific microservice
* Deceasing the risk of bloated workflows which reduces maintainability of the code as it is easy to chain states/functions in a one or more subflows

## Roadmap

### Backend
#### Version 1 Tasks
* [ ] .Net Framework Support
* [ ] General Queue Handler for servier side messaging
  * [ ] Inbound
  * [ ] Outbound
* [ ] Quartz Setup
* [ ] Quartzman Integration
* [ ] Nuget Release

#### Version 2 or more
* [ ] .Net Core Support
* [ ] Plugin support with loader
* [ ] Multi tenant support
* [ ] Subflow handling
* [ ] Documentation creation
* [ ] Quartz IJobStore hybrid store InMemory with persist possibility
* [ ] Quartzmin IJob dll upload and handling
* [ ] Include JavaScript in Nuget

### Frontend
#### Version 1 Tasks
* [ ] JavaScript XState
* [ ] JavaScript Workflow
* [ ] npm release

#### Version 2 or more
* [ ] Javascript Workflow designer
* [ ] Online Script Editor
