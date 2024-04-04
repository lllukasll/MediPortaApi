
using MediPortaApi.Helpers;
using MediPortaApi.Repositories;
using MediPortaApi.Repositories.Interfaces;
using MediPortaApi.Services;
using MediPortaApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace MediPortaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddTransient<IStackOverflowAPIService, StackOverflowAPIService>();

            builder.Services.AddHttpClient<IStackOverflowAPIService, StackOverflowAPIService>
                ().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite(builder.Configuration.GetConnectionString("MediPortaApiDB")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            app.ConfigureExceptionHandler(app.Logger);

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigrations();
            
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            
        }
    }
}
