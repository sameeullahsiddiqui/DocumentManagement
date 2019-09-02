using Autofac;
using DocumentApp.Core.Data.Repositories;
using DocumentApp.Data.Repositories;
using System.Reflection;

namespace DocumentApp.API.Capsule.Modules
{
    public class RepositoryCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("DocumentApp.Data"))
                                          .Where(_ => _.Name.EndsWith("Repository")).
                                          AsImplementedInterfaces().
                                          InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
        }

    }
}