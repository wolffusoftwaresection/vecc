using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Core.AutofacManager
{
    public class ContainerManager
    {
        public static IContainer Container;
        public static T Resolve<T>()
        {
            if (Container != null)
            {
                return Container.Resolve<T>();
            }
            return default(T);
        }
        public static object ResolveUnregistered(Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Container.Resolve(parameter.ParameterType);
                        if (service == null) throw new ArgumentException("Unkown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch
                {

                }
            }
            throw new ArgumentException("No contructor was found that had all the dependencies satisfied.");
        }
    }
}
