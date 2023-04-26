using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireBasicAuthenticationFilter;
using HangFireWebApi.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HangFireWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHangfire(configuration => configuration
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
            .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            builder.Services.AddHangfireServer();
            builder.Services.AddTransient<IServiceManagement, ServiceManagement>();
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
            app.UseHangfireDashboard("/hangfire",new DashboardOptions()
            {
                DashboardTitle = "Driver Dashboard",
                Authorization  = new []
                {
                    new HangfireCustomBasicAuthenticationFilter()
                    {
                        Pass = "Password",
                        User = "abcd"
                    }
                }
            });
            app.MapHangfireDashboard();
            RecurringJob.AddOrUpdate<IServiceManagement>(x=>x.SyncData(),"0 * * ? * *");
            app.Run();
        }
    }
}