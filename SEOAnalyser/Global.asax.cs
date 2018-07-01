using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using SEOAnalyser.Business.Interfaces;
using SEOAnalyser.Mapper;
using SEOAnalyser.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SEOAnalyser.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			ConfigureIoC();
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private static void ConfigureIoC()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(typeof(MvcApplication).Assembly);

			//RegisterDataMethods(builder);
			RegisterBusinessMethods(builder);
			RegisterAutoMapper(builder);

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
		private static void RegisterBusinessMethods(ContainerBuilder builder)
		{
			builder.RegisterType<ValidateService>().As<IValidateService>();
			builder.RegisterType<AnalysisService>().As<IAnalysisService>();
		}

		private static void RegisterAutoMapper(ContainerBuilder builder)
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingProfile());
			});

			builder.RegisterInstance(mapperConfiguration.CreateMapper()).As<IMapper>();
		}
	}
}
