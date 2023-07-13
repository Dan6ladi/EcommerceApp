using System.Text;
using ChoiceApp.ServiceBus;
using Microsoft.OpenApi.Models;
using ChoiceApp.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using ChoiceApp.Infrastructure.Models;
using ChoiceApp.SharedKernel.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChoiceApp.Persistence.Repository.Context;
using ChoiceApp.Infrastructure;
using ChoiceApp.Persistence;
using ChoiceApp.Filter;

namespace ChoiceApp
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
            //builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            string connStr = builder.Configuration.GetConnectionString("myconn");
            string key = builder.Configuration.GetSection("AppSettings").GetValue<string>("Token");
            builder.Services.AddDbContext<ChoiceContext>(Options =>
            {
                Options.UseSqlServer(connStr);
            });
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme(\"bearer{token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                });
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Luxurious.Api", Version = "v1" });
                //
#if DEBUG

#else
    options.DocumentFilter<SwaggerFilter>(); 
#endif
                options.OperationFilter<SecurityRequirementsOperationFilter>();

            });
           builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
            builder.Services.ApplicationServiceCollections();
            builder.Services.InfrastructureServiceCollections(builder.Configuration);
            builder.Services.PersistenceCollections();
            builder.Services.AddSingleton(typeof(ILogger), typeof(Logger<Program>));

            var app = builder.Build();


//#if DEBUG
//#else
//            var customerCreatedListener = app.Services.GetService<ICustomerCreatedListener>();
//            customerCreatedListener.RegisterOnMessageHandlerAndReceiveMessages();
//#endif


            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Luxurious.Api");
                });
            }


            app.UseMiddleware<ExceptionMiddleWare>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();


            app.Run();
        }
    }
}