using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductsAPI.Data;
using ProductsAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ProductsAPI.Config
{
    public class ProductsAPIApp
    {
        private readonly WebApplicationBuilder builder;
        private WebApplication app;

        public ProductsAPIApp(string[] args)
        {
            builder = WebApplication.CreateBuilder(args);

            Setup();
        }

        private void Setup()
        {
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            ConfigureDb();

            AddAuth();

            app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapIdentityApi<CustomUser>();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }

        private void ConfigureDb()
        {
            var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");

            var version = new MySqlServerVersion(new Version(8, 2, 0));

            builder.Services.AddDbContext<ProductsAPIDbContext>(options => options.UseMySql(connectionString, version));
        }

        private void AddAuth()
        {
            builder.Services.AddAuthorization();

            builder.Services.AddIdentityApiEndpoints<CustomUser>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                }
            }).AddEntityFrameworkStores<ProductsAPIDbContext>();
        }

        public void Run()
        {
            app.Run();
        }
    }
}
