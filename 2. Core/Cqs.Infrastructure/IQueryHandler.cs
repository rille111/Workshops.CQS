using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// A handler, that expects that the query (TQuery) to be something that delivers a result (TResult).
    /// </summary>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }

    //public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    //{
    //    Task<TResult> Handle(TQuery query);
    //}

}