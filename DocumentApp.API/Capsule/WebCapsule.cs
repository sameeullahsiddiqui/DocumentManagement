using Autofac;
using Autofac.Integration.WebApi;
using DocumentApp.API.Capsule.Modules;
using DocumentApp.Core.Data;
using DocumentApp.Core.Logging;
using DocumentApp.Data;
using DocumentApp.Logging.Logging;
using System.Web.Http;

namespace DocumentApp.API.Capsule
{
    public class WebCapsule
    {
        public void Initialise(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();

            //builder.RegisterFilterProvider();

            const string nameOrConnectionString = "name=DocumentAppDbConnection";
            builder.Register<IDbContext>(b =>
            {
                var logger = b.ResolveOptional<ILogger>();
                var context = new DocumentAppDbContext(nameOrConnectionString, logger);
                return context;
            }).InstancePerLifetimeScope();


            builder.Register(b => NLogLogger.Instance).SingleInstance();
            builder.RegisterModule<RepositoryCapsuleModule>();
            builder.RegisterModule<ServiceCapsuleModule>();
            builder.RegisterModule<ControllerCapsuleModule>();
            builder.RegisterModule<AutoMapperModule>();

            var container = builder.Build();

            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }
    }
}