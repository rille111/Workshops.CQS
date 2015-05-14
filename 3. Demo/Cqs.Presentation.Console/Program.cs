using System.Threading.Tasks;
using Cqs.Console.Utils;
using Cqs.Domain.Commands;
using Cqs.Domain.Models;
using Cqs.Domain.Queries;
using Cqs.Infrastructure;
using Cqs.Infrastructure.Decorators;
using Cqs.Infrastructure.Implementations;
using Cqs.Infrastructure.PubSub;
using Cqs.Persistence.RuntimeCache;

namespace Cqs.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DoPubsubTests();
            
            //SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            WorkWithCommandsWithoutIoc();
            IoC.ResetEverything();
            IoC.RegisterEverything();
            WorkWithCommandsWithIoc();
            WorkWithQueryProcessor();
            WorkWithQueryProcessorAndAsync();
            System.Console.WriteLine("\n-- Press any key to quit --");
            System.Console.ReadKey();
        }

        private static void DoPubsubTests()
        {
            System.Console.WriteLine("\n-- Pub/Sub tests --");
            var broker = new PubSub();
            broker.WorkInAsync = true;

            broker.Subscribe("NewsPosted", (data) =>
            {
                System.Console.WriteLine("Topic: 'NewsPosted' Data = '{0}'", data);
            });
            broker.Publish("NewsPosted", "yup");
            
        }

        private static void WorkWithCommandsWithoutIoc()
        {
            System.Console.WriteLine("\n-- No IoC --");

            // Line up some commands to execute
            var itemToCreateThenChange = new Todo { Caption = "Feed cat", Category = "Etc" };
            var cmd1 = new CreateTodoCommand { Data = new Todo { Caption = "I SHOULD BE DELETED", Category = "Etc" } }; // id will become 1
            var cmd2 = new CreateTodoCommand { Data = itemToCreateThenChange }; // id will become 2
            var cmd3 = new CreateTodoCommand { Data = new Todo { Caption = "Pay bills", Category = "Etc" } }; // id will become 3
            var cmd4 = new CreateTodoCommand { Data = new Todo { Caption = "I DO NOT PASS THE FILTER", Category = "Uncategorized" } };// id will become 3
            var updateCommand = new UpdateTodoCommand { Data = new Todo { Id = itemToCreateThenChange.Id, Category = itemToCreateThenChange.Category, Caption = "Feed BOTH cats" } };
            var deleteCommand = new DeleteTodoCommand { Id = 1 }; // delete milk 

            // Prepare the handlers
            var createHandler = new CreateTodoCommandHandler(new CachedRepository<Todo>());
            var updateHandler = new UpdateTodoCommandHandler(new CachedRepository<Todo>());
            var deleteHandler = new DeleteTodoCommandHandler(new CachedRepository<Todo>());
            // Example using a decorator. We're instantiating the decorator manually here but the idea is to it with IoC.
            // This decorator outputs some logging to to Console.Out.
            var decorating_deleteHandler = new LoggingCommandHandlerDecorator<DeleteTodoCommand>(deleteHandler, (ILogger)new ConsoleLogger());

            // Create something then queue it for deletion
            createHandler.Handle(cmd1).Wait();
            deleteCommand.Id = cmd1.Output;

            Task.WaitAll(
                createHandler.Handle(cmd2)
                , createHandler.Handle(cmd3)
                , createHandler.Handle(cmd4)
                );

            updateHandler.Handle(updateCommand).Wait();
            decorating_deleteHandler.Handle(deleteCommand).Wait();

            // Example using a queryhandler to deliver results
            var qhandler = new FindTodosByCategoryQueryHandler(new CachedRepository<Todo>());
            var query = new FindTodosByCategoryQuery { Category = "Etc" };
            var queryResult = qhandler.Handle(query);

            // Only two items should show up since we deleted one, and one doesn't match the query.
            PrintResults(queryResult.Result);

        }

        private static void WorkWithCommandsWithIoc()
        {

            System.Console.WriteLine("\n-- WITH IoC --");

            // Get handlers by manual invocation of IoC
            var createHandler = IoC.ThisContainer.GetInstance<ICommandHandler<CreateTodoCommand>>();
            var qhandler = IoC.ThisContainer.GetInstance<IQueryHandler<FindTodosByCategoryQuery, Todo[]>>();

            // Line up some commands and a query
            var cmd1 = new CreateTodoCommand { Data = new Todo { Caption = "Tidy room", Category = "Etc" } };
            var cmd2 = new CreateTodoCommand { Data = new Todo { Caption = "Do the dishes", Category = "Etc" } };
            var cmd3 = new CreateTodoCommand { Data = new Todo { Caption = "Swipe the floors", Category = "Etc" } };
            var query1 = new FindTodosByCategoryQuery { Category = "Etc" };

            // Stand handling
            Task.WaitAll(
            createHandler.Handle(cmd1),
            createHandler.Handle(cmd2),
            createHandler.Handle(cmd3)
            );
            var results = qhandler.Handle(query1).Result;

            PrintResults(results);
        }

        private static void WorkWithQueryProcessor()
        {
            System.Console.WriteLine("\n-- Using QueryProcessor --");

            var queryProcessor = new QueryProcessor();
            var cmdProcessor = new CommandProcessor();

            // Run a query with help of the query processor
            var query = new FindTodosByCategoryQuery { Category = "Etc" };
            var queryResult = queryProcessor.Handle(query).Result;

            // Run a delete command with the command processor and see how the decorator is used
            var delCommand = new DeleteTodoCommand() { Id = queryResult[0].Id };
            cmdProcessor.Handle(delCommand);

            PrintResults(queryResult);
        }

        private async static void WorkWithQueryProcessorAndAsync()
        {
            System.Console.WriteLine("\n-- Best of all worlds --");
            var broker = IoC.ThisContainer.GetInstance<IPubSub>();
            broker.Subscribe(typeof(CreateTodoCommand).Name, o => System.Console.WriteLine("Pub/Sub activation: CreateTodoCommand: {0} ", o));
            broker.Subscribe(typeof(FindTodosByCategoryQuery).Name, o => System.Console.WriteLine("Pub/Sub activation: FindTodosByCategoryQuery: {0} ", o));

            var cmd1 = new CreateTodoCommand { Data = new Todo { Caption = "Cowboy: Good", Category = "Cowboy" } };
            var cmd2 = new CreateTodoCommand { Data = new Todo { Caption = "Cowboy: Bad ", Category = "Cowboy" } };
            var cmd3 = new CreateTodoCommand { Data = new Todo { Caption = "Cowboy: Ugly", Category = "Cowboy" } };

            var cmdProcessor = new CommandProcessor();
            var queryProcessor = new QueryProcessor();

            // Force wait on all, but you can just as well just "await" them
            Task.WaitAll(
                cmdProcessor.Handle(cmd1),
                cmdProcessor.Handle(cmd2)
                );

            await cmdProcessor.Handle(cmd3).ContinueWith(p =>
            {
                // This code below will be executed truly async
                System.Console.WriteLine("- async: cmd3 handled!, this is the continuation. Cmd3.Output = " + cmd3.Output);
            });

            var query = new FindTodosByCategoryQuery { Category = "Cowboy" };

            // This code below will be executed truly async
            System.Console.WriteLine("- async: results of a find query -");
            var results = await queryProcessor.Handle(query);
            PrintResults(results);

        }

        private static void PrintResults(Todo[] results)
        {
            foreach (var todo in results)
                System.Console.WriteLine(todo.Caption);
        }
    }
}
