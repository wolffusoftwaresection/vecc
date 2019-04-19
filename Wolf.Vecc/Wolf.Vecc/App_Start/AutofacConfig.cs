using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Wolf.Vecc.Core.Cache;
using Wolf.Vecc.Data.DataContext;
using Wolf.Vecc.Data.DataService;

namespace Wolf.Vecc.App_Start
{
    public class AutofacConfig
    {
        public static void Register()
        {
            #region IOC注入
            var builder = new ContainerBuilder();
            //注册数据持久层
            //builder.Register(o => new DbServiceContext()).InstancePerHttpRequest();
            
            /// <summary>
            /// 对应update1 弃用注册ioc持久层时传入对象改用县城内唯一
            /// </summary>
            builder.Register(o => new DbServiceReposity(new VeccContext())).As<IDbServiceReposity>().InstancePerLifetimeScope();
            //注册所有controller
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            #region 缓存
            builder.RegisterType<CacheManager>().As<ICacheManager>().SingleInstance();
            #endregion
            #region 自动注入SERVICES
            var interfaces = Assembly.Load("Wolf.Vecc.IService");
            var services = Assembly.Load("Wolf.Vecc.Service");
            builder.RegisterAssemblyTypes(interfaces, services)
                    .Where(s => s.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
            #endregion

            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion
        }
    }
}