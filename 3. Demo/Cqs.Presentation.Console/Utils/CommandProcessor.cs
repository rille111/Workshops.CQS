using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cqs.Infrastructure;

namespace Cqs.Console.Utils
{
    sealed class CommandProcessor : ICommandProcessor
    {
        public async Task Handle(ICommand command)
        {

            var handlerType = typeof(ICommandHandler<>)
                .MakeGenericType(command.GetType());

            dynamic handler = IoC.ThisContainer.GetInstance(handlerType);

            await handler.Handle((dynamic)command);            
        }
    }

    //sealed class CommandProcessorWithChainedHandlers
    //{
    //    public List<ICommandHandler<ICommand>> CommandHandlers { get; set; }

    //    public CommandProcessorWithChainedHandlers()
    //    {
    //        CommandHandlers = new List<ICommandHandler<ICommand>>();
    //    }

    //    public async Task Handle(ICommand command)
    //    {
            


    //        var handlerType = typeof(ICommandHandler<>)
    //            .MakeGenericType(command.GetType());

    //        dynamic handler = IoC.ThisContainer.GetInstance(handlerType);

    //        await handler.Handle((dynamic)command);
    //    }
    //}
}