namespace Cqs.Infrastructure
{
    /// <summary>
    /// A command. It works like a marker interface for CommandHandlers. 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// This is a cool invention of mine. Output can be anything!
        /// Since we're using a dynamic here instead of demanding a generic type in the interface signature, 
        /// we can more easily implement CommandProcessors, and Decorators can access Output easily.
        /// Another idea is to use ICommandResult, but that will increase complexity, a lot.
        /// </summary>
        dynamic Output { get; set; }
    }
}
