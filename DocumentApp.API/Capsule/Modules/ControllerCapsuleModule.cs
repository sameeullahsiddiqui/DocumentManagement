using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace DocumentApp.API.Capsule.Modules
{
    public class ControllerCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.Load("DocumentApp.API"));
        }
    }
}