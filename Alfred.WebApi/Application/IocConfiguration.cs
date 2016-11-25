﻿using System.Reflection;
using System.Web.Http;
using Alfred.IoC;
using LightInject;

namespace Alfred.WebApi.Application
{
    public static class IocConfiguration
    {
        public static HttpConfiguration ConfigIoC(this HttpConfiguration config)
        {
            var container = new ServiceContainer();
            container.RegisterAssembly(Assembly.GetAssembly(typeof(Startup)));
            container.RegisterFrom<WebApiCompositionRoot>();
            container.RegisterApiControllers();
            container.EnableWebApi(config);
            return config;
        }
    }
}
