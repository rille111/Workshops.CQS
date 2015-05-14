using System.Threading.Tasks;

namespace Cqs.Infrastructure.Implementations
{
    public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _decorated;
        private readonly ILogger _logger;

        public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public async Task Handle(TCommand command)
        {
            await Task.Run( async () =>
            {
                var name = command.GetType().FullName;
                // must await the inner Handle() task, so that we can access command.Output
                await _decorated.Handle(command);
                _logger.Write(LoggerLevel.Info, "Handled Command: {0}, with output: {1}", name, command.Output);
            });
        }
    }
}