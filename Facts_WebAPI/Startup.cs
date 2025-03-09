using Facts_Business.Repository;
using Facts_Business.Repository.IRepository;
using Facts_Common;
using Facts_DataAccess;
using Facts_DataAccess.DbInitializer;
using Facts_WebAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Facts_WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            SD.SqlCommandConnectionTimeout = int.Parse(Configuration.GetValue<string>("SqlCommandConnectionTimeout"));

            services.AddControllers().AddMvcOptions(x =>
                x.SuppressAsyncSuffixInActionNames = false);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // include API xml documentation
                var apiAssembly = typeof(FactsController).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(apiAssembly));

                apiAssembly = typeof(CommentsController).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(apiAssembly));

                apiAssembly = typeof(StatesController).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(apiAssembly));

                // include models xml documentation
                var modelsAssembly = typeof(Facts_Models.FactsModels.State.StateItemCreateRequest).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(modelsAssembly));
                modelsAssembly = typeof(Facts_Models.FactsModels.State.StateItemResponse).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(modelsAssembly));
                modelsAssembly = typeof(Facts_Models.FactsModels.State.StateItemUpdateRequest).Assembly;
                c.IncludeXmlComments(GetXmlDocumentationFileFor(modelsAssembly));

                //c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Facts sevice API (Library)", Version = "v2" });

            });

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IFactRepository, FactRepository>();
            services.AddScoped<IFactCommentRepository, FactCommentRepository>();
            services.AddScoped<IStateRepository, StateRepository>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("FactsDBPostgresSQLConnection"),
                u => u.CommandTimeout(SD.SqlCommandConnectionTimeout));
                options.UseLazyLoadingProxies();
            });


            services.AddOpenApiDocument(options =>
            {
                options.Title = "Facts (Library API Doc)";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                //app.UseSwagger(c =>
                //{
                //    c.PreSerializeFilters.Add((swagger, httpReq) =>
                //    {
                //        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Host}" } };
                //    });
                //});

                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();

            app.UseSwaggerUI(x =>
            {
                x.DocExpansion(DocExpansion.List);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbInitializer.InitializeDb();
        }

        private static string GetXmlDocumentationFileFor(Assembly assembly)
        {
            var documentationFile = $"{assembly.GetName().Name}.xml";
            var path = Path.Combine(AppContext.BaseDirectory, documentationFile);

            return path;
        }

    }
}
