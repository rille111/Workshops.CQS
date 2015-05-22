using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqs.Infrastructure
{
    /// <summary>
    /// Different people use different IoC Containers.
    /// This adapter can be set up in your client project. See example below.
    /// See source: https://github.com/ServiceStack/ServiceStack/wiki/The-IoC-container
    /// </summary>
    public interface IContainerAdapter
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
    }


    /*
    public class SimpleInjectorContainerAdapter : IContainerAdapter
    {
        private readonly SimpleInjector.Container container;

        public object Resolve(Type type)
        {
            var instance = container.GetInstance<T>();
            return instance;
        }

        public T Resolve<T>() where T : class
        {
            var instance = container.GetInstance<T>();
            return instance;
        }
    }
    */
}
