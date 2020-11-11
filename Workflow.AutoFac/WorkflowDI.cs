using Akka.Actor;
using Akka.Quartz.Actor;
using Autofac;
using DevelApp.Workflow.Actors;
using DevelApp.Workflow.TopActorProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace Workflow.AutoFac
{
    public class WorkflowDI
    {
        /// <summary>
        /// Registers ActorSystem and adds to level 
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void ConfigureServices(IServiceCollection serviceCollection, string actorSystemName)
        {
            // Configuration is loaded from either App.config or Web.config
            serviceCollection.AddSingleton(_ => ActorSystem.Create(actorSystemName));

            //Configuration is loaded from C:\Workflow\MonsterSystem.config
            //serviceCollection.AddSingleton(_ => ActorSystem.Create(actorSystemName, DevelApp.Workflow.Utilities.ConfigurationLoader.LoadFromDisc(@"C:\Workflow\MonsterSystem.config")));

            serviceCollection.AddSingleton<DataOwnerCoordinatorActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var dataOwnerCoordinatorActor = actorSystem.ActorOf(Props.Create(() => new DataOwnerCoordinatorActor()));
                return () => dataOwnerCoordinatorActor;
            });

            serviceCollection.AddSingleton<DataServiceCoordinatorActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var dataServiceCoordinatorActor = actorSystem.ActorOf(Props.Create(() => new DataServiceCoordinatorActor()));
                return () => dataServiceCoordinatorActor;
            });

            serviceCollection.AddSingleton<DeadletterActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var deadletterActor = actorSystem.ActorOf(Props.Create(() => new DeadletterActor(1)));
                return () => deadletterActor;
            });

            serviceCollection.AddSingleton<UserCoordinatorActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var userCoordinatorActor = actorSystem.ActorOf(Props.Create(() => new UserCoordinatorActor()));
                return () => userCoordinatorActor;
            });

            serviceCollection.AddSingleton<WorkflowControllerCoordinatorActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var workflowControllerCoordinatorActor = actorSystem.ActorOf(Props.Create(() => new WorkflowControllerCoordinatorActor()));
                return () => workflowControllerCoordinatorActor;
            });

            serviceCollection.AddSingleton<QuartzPersistentActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var quartzPersistentActor = actorSystem.ActorOf(Props.Create(() => new QuartzPersistentActor("QuartzScheduler")), "QuartzActor");
                return () => quartzPersistentActor;
            });
        }

        /// <summary>
        /// Registers Graceful starup and shutdown of the service ActorSystem
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ContainerBuilder builder)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>(); // start Akka.NET
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait();
            });
            builder.RegisterType<ConfigurationActor>();
            builder.RegisterType<DataOwnerActor>();
            builder.RegisterType<DataServiceControllerActor>();
            builder.RegisterType<DataServiceWebhookActor>();
            builder.RegisterType<JsonSchemaActor>();
            builder.RegisterType<ModuleActor>();
            builder.RegisterType<SagaActor>();
            builder.RegisterType<TranslationActor>();
            builder.RegisterType<TranslationLanguageActor>();
            builder.RegisterType<UserActor>();
            builder.RegisterType<WorkflowActor>();
            builder.RegisterType<WorkflowControllerActor>();
        }
    }
}
