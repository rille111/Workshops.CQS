namespace Cqs.Infrastructure
{
    public interface ILogger
    {
        void Write(LoggerLevel level, string output);
        void Write(LoggerLevel level, string output, params object[] parameters);
    }

    public enum LoggerLevel
    {
        Debug, Info, Warning, Error, Fatal
    }
}
