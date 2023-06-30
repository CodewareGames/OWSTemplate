using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using OWSShared.Interfaces;
using OWSShared.Implementations;
using OWSShared.Middleware;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

namespace $safeprojectname$
{
    public class Startup
    {
        private Container container = new SimpleInjector.Container();

        public Startup(IConfiguration configuration)
        {
            container.Options.ResolveUnregisteredConcreteTypes = false;

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("./temp/DataProtection-Keys"));

            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddSimpleInjector(container, options => {
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Open World Server $safeprojectname$", Version = "v1" });

                c.AddSecurityDefinition("X-CustomerGUID", new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the X-CustomerGUID scheme",
                    Name = "X-CustomerGUID",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "X-CustomerGUID"
                });

                c.OperationFilter<SwaggerSecurityRequirementsDocumentFilter>();

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "$safeprojectname$.xml");
                c.IncludeXmlComments(filePath);
            });

            InitializeContainer(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSimpleInjector(container);

            app.UseMiddleware<StoreCustomerGUIDMiddleware>(container);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Open World Server $safeprojectname$");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            container.Verify();
        }

        private void InitializeContainer(IServiceCollection services)
        {
            container.Register<IHeaderCustomerGUID, HeaderCustomerGUID>(Lifestyle.Scoped);
        }
    }
}
