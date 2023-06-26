using Microsoft.EntityFrameworkCore;
using TiendaApi.Data;
using TiendaApi.Interfaces;
using Newtonsoft.Json;

namespace TiendaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Agregar DbContext
            builder.Services.AddDbContext<Tienda9CDbContext>(x => x.UseMySql("name=ConnectionStrings:DefaultConnection", new MySqlServerVersion("8.0.30")));
            //Agregar repositorios
            builder.Services.AddScoped<IFabricanteRepository,FabricanteRepository>();
            builder.Services.AddScoped<IProductoRepository,ProductoRepository>();
            // Agregar los controladores y modificar el Serializador.
            builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore);
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}