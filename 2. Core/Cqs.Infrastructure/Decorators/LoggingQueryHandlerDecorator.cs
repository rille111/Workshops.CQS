using System.Threading.Tasks;

namespace Cqs.Infrastructure.Implementations
{
    public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;
        private readonly ILogger _logger;

        public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public Task<TResult> Handle(TQuery command)
        {
            var name = command.GetType().FullName;
            _logger.Write(LoggerLevel.Info, "Handling Query: {0}", name);
            var result = _decorated.Handle(command);
            return result;
        }
    }
}