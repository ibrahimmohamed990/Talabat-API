using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.API.Extensions;
using Store.API.Helper;
using Store.API.MiddleWares;
using Store.Data.Context;

namespace Store.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });

            });
            builder.Services.AddDbContext<StoreIdentityDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                    return ConnectionMultiplexer.Connect(configration);
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", policy =>
            //    {
            //        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200", "https://localhost:44337");
            //    });
            //});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyMvcApp",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7239/")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();
            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleWare>();

            //app.UseStatusCodePagesWithRedirects("/errors/{0}");
            //  >> with redirect uses two requests to handle the result , so it slow. 
            //app.UseStatusCodePagesWithReExecute("/errors/{0}");
            //  >> with reExecute use one request to handle the result , so it faster.

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            //app.UseCors("CorsPolicy");
            app.UseCors("AllowMyMvcApp");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
