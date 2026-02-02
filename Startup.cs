using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using joelbyford;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace CsvToJsonCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o => o.InputFormatters.Insert(0, new CsvInputFormatter()));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CsvToJsonCore", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //always serve Swagger UI at /swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CsvToJsonCore v1"));

            //Insert the Basic Authentication Middleware handler *ONLY IF* it was enabled in appsettings.json
            bool basicAuthEnabled = this.Configuration.GetValue<bool>("AppSettings:BasicAuth:Enabled");
            
            if (basicAuthEnabled)
            {   
                //Uses values from "BasicAuth" under "AppSettings" in the appsettings.json
                String basicAuthRealm = this.Configuration.GetValue<String>("AppSettings:BasicAuth:Realm");
                String basicAuthUserJson = this.Configuration.GetValue<String>("AppSettings:BasicAuth:UsersJson");

                // Using the BasicAuth NuGet package from https://github.com/joelbyford/BasicAuth
                Dictionary<string, string> basicAuthUsers = new Dictionary<string, string>();
                var packageJson = File.ReadAllText(basicAuthUserJson);
                basicAuthUsers = JsonSerializer.Deserialize<Dictionary<string, string>>(packageJson);
                app.UseMiddleware<joelbyford.BasicAuth>(basicAuthRealm, basicAuthUsers);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
