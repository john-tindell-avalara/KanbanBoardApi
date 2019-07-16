using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web.Http;
using FluentValidation;
using KanbanBoardApi.Commands;
using KanbanBoardApi.Commands.Services;
using KanbanBoardApi.Dto;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.HyperMedia;
using KanbanBoardApi.HyperMedia.States;
using KanbanBoardApi.Mapping;
using KanbanBoardApi.Queries;
using KanbanBoardApi.Validation;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace KanbanBoardApi
{
    public class SimpleInjectorConfig
    {
        public static void Register(Container container)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterCollection(typeof (IHyperMediaState), typeof (IHyperMediaState).Assembly);
            container.Register<IBoardState, BoardState>();
            container.Register<IBoardTaskState, BoardTaskState>();
            container.Register<IBoardColumnState, BoardColumnState>();

            container.Register<ISlugService, SlugService>();

            container.Register<ILinkFactory, LinkFactory>();
            container.Register<IHyperMediaFactory, HyperMediaFactory>();
            container.Register<IMappingService, MappingService>();

            container.Register<IDataContext, DataContext>();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.EnableHttpRequestMessageTracking(GlobalConfiguration.Configuration);
            container.RegisterSingleton<IRequestMessageProvider>(new RequestMessageProvider(container));

            container.Register<IValidator<Board>, BoardValidator>();
            container.Register<IValidator<BoardColumn>, BoardColumnValidator>();
            container.Register<IValidator<BoardTask>, BoardTaskValidator>();

            container.RegisterSingleton<IMediator, Mediator>();
            var assemblies = GetAssemblies().ToArray();
            container.Register(typeof(IRequestHandler<,>), assemblies);

            var notificationHandlerTypes = container.GetTypesToRegister(typeof(INotificationHandler<>), assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });
            container.Collection.Register(typeof(INotificationHandler<>), notificationHandlerTypes);
            container.Collection.Register(typeof(IPipelineBehavior<,>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPreProcessor<>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPostProcessor<,>), Enumerable.Empty<Type>());

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            //container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            yield return typeof(GetBoardBySlugQuery).GetTypeInfo().Assembly;
            yield return typeof(CreateBoardColumnCommand).GetTypeInfo().Assembly;
        }
    }
}