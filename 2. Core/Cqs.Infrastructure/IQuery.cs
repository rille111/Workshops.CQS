namespace Cqs.Infrastructure
{
    /// <summary>
    /// A query that delivers TResult when it's done. This can be seen as a "Parameters" object that a handler will take care of.
    /// See http://stackoverflow.com/questions/14420276/well-designed-query-commands-and-or-specifications  
    /// </summary>
    public interface IQuery<TResult>
    {
    }
}