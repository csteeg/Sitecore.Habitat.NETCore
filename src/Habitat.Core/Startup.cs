using Habitat.Core.Repositories.Faq;
using Habitat.Core.Repositories.Identity;
using Habitat.Core.Repositories.Metadata;
using Habitat.Core.Repositories.Multisite;
using Habitat.Core.Repositories.Navigation;
using BoC.CmsHelpers.Sitecore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Feature.Language.Infrastructure.Repositories;
using Sitecore.Feature.Navigation.Repositories;

namespace coretest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiSettings>(Configuration.GetSection("AppSettings:SitecoreEndPoint"));

            // Add framework services.
            services.AddMvc();
            services.AddLogging();
            services.AddOptions();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSitecore();

            // Add application services.
            services.AddTransient<INavigationRepository, NavigationRepository>();
            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddTransient<IGroupMemberRepository, GroupMemberRepository>();
            services.AddTransient<IMetadataRepository, MetadataRepository>();
            services.AddTransient<IMultisiteRepository, MultisiteRepository>();
            services.AddTransient<ILanguageRepository, LanguageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{*pathInfo}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
