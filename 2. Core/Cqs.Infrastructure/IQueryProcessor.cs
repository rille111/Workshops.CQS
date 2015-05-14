using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// You can use a queryprocessor to avoid bloated constructors that has zounds of query/command handlers.
    /// It is the responsibility of the implementation of the IQueryProcessor to find the right IQueryHandler. (Often with help of IoC)
    /// </summary>
    /// <remarks>
    /// Example, see http://stackoverflow.com/questions/14420276/well-designed-query-commands-and-or-specifications#
    /// </remarks>
    public interface IQueryProcessor
    {
        Task<TResult> Handle<TResult>(IQuery<TResult> query);
    }

    // I have my own recommended implementation that supports Async, and relies on SimpleInjector:

    /*
    /// <remarks>
    ///         * The QueryProcessor class constructs a specific IQueryHandler<TQuery, TResult> type based on the type of the supplied query instance. 
    ///         * This type is used to ask the supplied container class to get an instance of that type. 
    ///         * Unfortunately we need to call the Handle method using reflection (by using the C# 4.0 dymamic keyword in this case), 
    ///         * because at this point it is impossible to cast the handler instance, since the generic TQuery argument is not available at compile time. 
    ///         * However, unless the Handle method is renamed or gets other arguments, 
    ///         * this call will never fail and if you want to, it is very easy to write a unit test for this class. 
    ///         * Using reflection will give a slight drop, but is nothing to really worry about.
    /// </remarks>
    sealed class QueryProcessor : IQueryProcessor
    {
        public Task<TResult> Handle<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = IoC.Instance.GetInstance(handlerType);

            // If you change the method name from 'Handle' - this method will crash.
            return handler.Handle((dynamic)query);
        }
    }
    */   
}