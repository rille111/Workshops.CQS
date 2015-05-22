using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// With help of an IocContainerAdapter (that MUST be set first!) you can execute commands and queries,
    /// and the corresponding Handler will be found and used to execute the query or command.
    /// Does not work with Xamarin due to the dynamic keyword.
    /// </summary>
    public class CommandQueryProcessors
    {
        private static CommandQueryProcessors _instance;
        public static IContainerAdapter IocContainerAdapter { get; set; }
        public static CommandQueryProcessors Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommandQueryProcessors();
                }
                return _instance;
            }
        }


        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));



            //dynamic handler = IoC.ThisContainer.GetInstance(handlerType);
            dynamic handler = IocContainerAdapter.Resolve(handlerType);

            // If you change the method name from 'Handle' - this method will crash.
            return await handler.Handle((dynamic)query);
        }

        public async Task Execute(ICommand command)
        {

            var handlerType = typeof(ICommandHandler<>)
                .MakeGenericType(command.GetType());

            //dynamic handler = IoC.ThisContainer.GetInstance(handlerType);
            dynamic handler = IocContainerAdapter.Resolve(handlerType);

            await handler.Handle((dynamic)command);
        }

    }
}
