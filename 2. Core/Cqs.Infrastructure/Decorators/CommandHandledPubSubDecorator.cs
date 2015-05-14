using System.Threading.Tasks;
using Cqs.Infrastructure.PubSub;

namespace Cqs.Infrastructure.Decorators
{
    /// <summary>
    ///  Takes the class name as the topic. Ie "class CreateTodoCommand" and the topic becomes "CreateTodoCommand".
    ///  command.Output gets set to message.Data.
    /// </summary>
    public class CommandHandledPubSubDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly IPubSub _pubSub;

        public CommandHandledPubSubDecorator(ICommandHandler<TCommand> decorated, IPubSub pubSub)
        {
            this._decorated = decorated;
            this._pubSub = pubSub;
        }

        public async Task Handle(TCommand command)
        {
            await Task.Run( async () =>
            {
                await this._decorated.Handle(command);
                this._pubSub.Publish(command.GetType().Name, command.Output);
            });
        }

    }
}
