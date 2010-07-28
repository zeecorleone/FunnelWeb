﻿using System.Web;
using System.Web.Configuration;
using Autofac;
using Autofac.Integration.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Web.Application.Installation;
using FunnelWeb.Web.Model.Repositories.Internal;
using NHibernate;

namespace FunnelWeb.Web.Model.Repositories
{
    public class RepositoriesModule : Module
    {
        private static ISessionFactory _sessionFactory;
        private static readonly object _lock = new object();
        
        public RepositoriesModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDatabase>().As<IApplicationDatabase>();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();

            builder.Register<IFileRepository>(x => new FileRepository(WebConfigurationManager.AppSettings["FunnelWeb.configuration.uploadpath"], x.Resolve<HttpServerUtilityBase>())).HttpRequestScoped();
            builder.RegisterType<FeedRepository>().As<IFeedRepository>().HttpRequestScoped();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>().HttpRequestScoped();
            builder.Register(x =>
            {
                if (_sessionFactory == null)
                {
                    lock (_lock)
                    {
                        if (_sessionFactory == null)
                        {
                            _sessionFactory = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(new ConnectionStringProvider().ConnectionString))
                                .Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()))
                                .BuildSessionFactory();
                        }
                    }
                }

                var session = _sessionFactory.OpenSession();
                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
