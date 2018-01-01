using ASW.Repositories;
using ASW.Repositories.Contracts;
using ASW.Repository;
using ASW.Services;
using ASW.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ASW
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
            // Registering InMemory Database Context Dependency. Scoped = one instance Per Request
            services.AddScoped(provider =>
                new ASWContext(new DbContextOptionsBuilder().UseInMemoryDatabase("ASW").Options));

            //Registering Controller Dependencies
            services.AddScoped<IDiffRepository, DiffRepository>();
            services.AddScoped<IDiffService, DiffService>();

            //Setting up Web API Framework -> in asp.net core MVC/API share the same engine
            services.AddMvc();

            //Setting up Swagger -> auto-gen tool that generates API documentation acessible on web browser
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Assigment Scalable Web API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();

            app.UseSwagger();

            //Configuring swagger to provide a json runtime-generated file. That contains API documentation data
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assigment Scalable Web API v1"); });
        }
    }
}