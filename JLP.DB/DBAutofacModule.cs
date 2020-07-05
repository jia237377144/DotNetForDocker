using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLP.DB
{
    public class DBAutofacModule: Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.Name.EndsWith("DAL"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

    }
}
