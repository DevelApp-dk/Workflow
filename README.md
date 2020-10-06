# Workflow
Workflow Orchestration using Quartz.Net as internal scheduler to make is easier to created uncoupled workflow with frontend, automation and integration

## Features

## Reasoning and definitions
When making selfservice applications, integrations and automations there is a high need to expose program flow for business users as application of business rules requires ability to reason about program flow without need for programming abilities for the business users.
Workflow is based on a mix of flow based programming and executable statecharts (orchistration of the individual workflows and the tasks in the workflow). 
* Flow based programming: In short flow based programming is a clear separation of control of flow and specific tasks in the flow making it easier to follow the flow and reason about the flow with non-software developers. 
* Statecharts: Statecharts is statemachines that support parallel and substate execution. Statecharts used is a subform of [SCXML](https://www.w3.org/TR/scxml/) which I call a sharable statechart where the specific actions are specified but not containing specific functions because that is programming handling more than flow and seems to muddle the ability to reason on the flow. The statecharts are used to execute the flow providing a "As Implemented" documentation of the program flow. Frontend definitions of workflows are delivered as XState defined statecharts.
* Workflow: Workflows are sometimes called orchistrations especially in Service Broker implementations. A Workflow composed of Steps and Transitions. The individual workflows can be triggered by a timed schedule or by interaction
* Step: Each step is specifying a specific handling based on the step properties and types. Each state uses a state action which is an object with one or more functions within defined by an interface to the statetype
  * Steptypes:
    * Automatic: Running without user interaction. Has three functions: [OnEntry], [Execute] and [OnExit].
    * User interaction: Running with user interaction. Has four backend functions: [OnEntry], [CreateOutput], [ValidateInput] and [OnExit] and on the frontend state handles the state execution based on output data and input data. The format of the input and output data is defined based on Json Schema making it easy to autogenerate and validate both on backend and on frontend. After the [CreateOutput] function is run the backend suspends the execution and a message based on SagaStep is sent to the frontend. When a message is received from the frontend Json Schema validation is run on the message form and if passing the [ValidateInput] is run to make the backend validation on the message content. If all validation is passed the state transition is done based on the Outcome decided in the frontend and a message is sendt to the frontend that transition for the next state is allowed. If one validation fails a message is sent to the user interface and the SagaStep is updated.
    * Subworkflow: Initiates one or more sub workflows. Has ? functions: [OnEntry]
  * Step Action: Individual executable code parts based on step types. This is made with .Net code but it would also be possible to expand with other languages
    * [OnEntry] for guard functionality and preprocessing
    * [Execute] for main execution responsible for deciding Outcome
    * [CreateOutput] for preparing the output to the user interface
    * [ValidateInput] validates the message content based on the selected Outcome
    * [OnExit] for post processing typically Saga update
* Saga: A sage is normally a long running transaction. Here the responsibility is for handling the placement of the workflow instance

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
