using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Cqs.Console.Utils;
using Cqs.Domain.Commands;
using Cqs.Domain.Queries;
using Cqs.Infrastructure;
using Cqs.Infrastructure.Decorators;
using Cqs.Infrastructure.Implementations;
using Cqs.Infrastructure.PubSub;
using Cqs.Persistence.RuntimeCache;
using SimpleInjector;
using SimpleInjector.Extensions;

public class IoC
{
    private static Container _containerInstance;
    private static readonly object _locker = new object();
    public static Container ThisContainer
    {
        get
        {
            if (_containerInstance != null)
                return _containerInstance;

            lock (_locker)
            {
                _containerInstance = new Container();
                //_Instance.Options.AllowOverridingRegistrations = true;
            }

            return _containerInstance;
        }
    }

    public static void RegisterEverything()
    {
        var domainAssembly = typeof(FindTodosByCategoryQueryHandler).Assembly;
        // Get all implementing (closing) types that exist in domainAssembly, for the open interface IQueryHandler
        var queryHandlerTypes = OpenGenericBatchRegistrationExtensions.GetTypesToRegister(ThisContainer,typeof (IQueryHandler<,>), domainAssembly);
        // Do the same here but since we got 2 simalar closing types, exclude one of them
        var commandHandlerTypes = OpenGenericBatchRegistrationExtensions.GetTypesToRegister(ThisContainer, typeof(ICommandHandler<>), domainAssembly)
            .Except(new[] {typeof(ChainedCreateTodoCommandHandler)});

        // Straight on registrations
        ThisContainer.Register<ILogger, ConsoleLogger>();
        ThisContainer.Register<IPubSub, PubSub>(Lifestyle.Singleton);
        
        // Register open generics to closed ones
        ThisContainer.RegisterOpenGeneric(typeof(IRepository<>), typeof(CachedRepository<>));
        
        // Register an interface that contain open generics, to all closing implementations - in a specific assembly or in a list of types
        ThisContainer.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), queryHandlerTypes);
        ThisContainer.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), commandHandlerTypes);

        // Register decorators, there is a specific order in how they are executed, don't remember what order.
        // More help here: http://stackoverflow.com/questions/10304023/simpleinjector-is-this-the-right-way-to-registermanyforopengeneric-when-i-have
        ThisContainer.RegisterDecorator(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        ThisContainer.RegisterDecorator(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        ThisContainer.RegisterDecorator(typeof(IQueryHandler<,>), typeof(QueryHandledPubSubDecorator<,>));

        // Earlier, I implemented a Chain of Responsibility pattern, but it was ugly.
        //ThisContainer.RegisterInitializer<CreateTodoCommandHandler>(handler =>
        //{
        //    var chained = ThisContainer.GetInstance<ChainedCreateTodoCommandHandler>();
        //    handler.SetSuccessor(chained);
        //} );
    }

    public static void ResetEverything()
    {
        _containerInstance = null;
    }
}
