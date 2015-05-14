using System.Threading.Tasks;

namespace Cqs.Infrastructure
{

    /// <summary>
    /// A handler for a command. 
    /// 
    /// </summary>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// *May* set command.Output when it's done.
        /// </summary>
        Task Handle(TCommand command);
    }
}