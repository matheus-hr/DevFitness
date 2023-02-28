using DevFitness.API.Persistence;
using DevFitness.API.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DevFitness.API
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
            services.AddAutoMapper(typeof(UserProfile)); //Serve para criar o automapper, e colocar o IMappper nos contrutores das Controllers

            //Configura a string de Conex�o
            string connectionString = Configuration.GetConnectionString("DevFitnessCs");

            if (connectionString.Contains("Data Source"))
            {
                if (connectionString.Contains("DiretorioRaizDoProjeto"))
                {
                    string diretorioArquivoDb = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
                    connectionString = connectionString.Replace("C:\\DiretorioRaizDoProjeto", diretorioArquivoDb);
                }

                services.AddDbContext<DevFitnessDbContext>(options => options.UseSqlite(connectionString));
            }
            else
            {
                services.AddDbContext<DevFitnessDbContext>(options => options.UseSqlServer(connectionString));
            }

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "DevFitness.API", 
                    Version = "v1",  
                    Contact = new OpenApiContact 
                    {
                        Name = "Matheus Henrique",
                        Url = new Uri("https://github.com/matheushr-dev")
                    }
                });

                //Configura a cria��o de comentarios no swagger.
                ////  Nao esquecer de que foi adicionado algumas linhas no arquivo da solution (.sln)
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevFitness.API v1"));
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
