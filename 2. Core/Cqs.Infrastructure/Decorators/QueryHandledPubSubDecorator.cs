using System.Threading.Tasks;
using Cqs.Infrastructure.PubSub;

namespace Cqs.Infrastructure.Decorators
{
    /// <summary>
    ///  Takes the class name as the topic. Ie "class FindTodosQuery" and the topic becomes "FindTodosQuery".
    ///  ???
    /// </summary>
    public class QueryHandledPubSubDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;
        private readonly IPubSub _pubSub;

        public QueryHandledPubSubDecorator(IQueryHandler<TQuery, TResult> decorated, IPubSub pubSub)
        {
            this._decorated = decorated;
            this._pubSub = pubSub;
        }

        public Task<TResult> Handle(TQuery query)
        {
            var task = this._decorated.Handle(query);
            task.ContinueWith((t) =>
            {
                this._pubSub.Publish(query.GetType().Name, t.Result);
            });
            return task;
        }
    }
}
