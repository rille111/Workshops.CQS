using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// You can use a commandprocessor to avoid bloated constructors that has zounds of query/command handlers.
    /// It is the responsibility of the implementation of the ICommandProcessor to find the right IQueryHandler. (Often with help of IoC)
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Command processors won't return anything but when processed, the command.Output *may* have been set.
        /// </summary>
        Task Handle(ICommand command);
    }

    //sealed class CommandProcessor : ICommandProcessor
    //{
    //    public async Task Handle(ICommand command)
    //    {

    //        var handlerType = typeof(ICommandHandler<>)
    //            .MakeGenericType(command.GetType());

    //        dynamic handler = IoC.Instance.GetInstance(handlerType);

    //        await handler.Handle((dynamic)command);
    //    }
    //}
}