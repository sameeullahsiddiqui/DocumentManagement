
using Autofac;
using AutoMapper;
using DocumentApp.Core.Entities.Foundation;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DocumentApp.API.Capsule.Modules
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataEntities = Assembly.Load("DocumentApp.Core").GetTypes().Where(x => typeof(IEntity).IsAssignableFrom(x)).ToList();
            var dtos = Assembly.Load("DocumentApp.Dto").GetTypes().Where(x => x.Name.EndsWith("Dto", true, CultureInfo.InvariantCulture)).ToList();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var entity in dataEntities)
                {
                    var matchingDto = dtos.FirstOrDefault(x => x.Name.Replace("Dto", string.Empty).Equals(entity.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (matchingDto != null)
                    {
                        cfg.CreateMap(entity, matchingDto);
                        cfg.CreateMap(matchingDto, entity);
                    }
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }

    }
}