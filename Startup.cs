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
using Microsoft.OpenApi.Models;
using BasicAuth;

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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CsvToJsonCore v1"));
            }
            //Insert the Basic Authentication Middleware handler *ONLY IF* it was enabled in appsettings.json
            bool basicAuthEnabled = this.Configuration.GetValue<bool>("AppSettings:BasicAuth:Enabled");
            
            if (basicAuthEnabled)
            {   
                //Uses values from "BasicAuth" under "AppSettings" in the appsettings.json
                String basicAuthRealm = this.Configuration.GetValue<String>("AppSettings:BasicAuth:Realm");
                String basicAuthUserJson = this.Configuration.GetValue<String>("AppSettings:BasicAuth:UsersJson");
                app.UseMiddleware<BasicAuth.BasicAuth>(basicAuthRealm, basicAuthUserJson);
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
