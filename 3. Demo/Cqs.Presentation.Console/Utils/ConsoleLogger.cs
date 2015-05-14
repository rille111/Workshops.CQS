using Cqs.Infrastructure;

namespace Cqs.Console.Utils
{
    public class ConsoleLogger : ILogger
    {
        public void Write(LoggerLevel level, string output)
        {
            System.Console.WriteLine("{0}: {1}", level, output);
        }

        public void Write(LoggerLevel level, string output, params object[] parameters)
        {
            System.Console.WriteLine("{0}: {1}", level, string.Format(output, parameters) );
        }
    }
}
