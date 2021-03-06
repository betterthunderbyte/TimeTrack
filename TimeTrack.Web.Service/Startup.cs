using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using TimeTrack.Core.Configuration;
using TimeTrack.UseCase;
using TimeTrack.Web.Service.Tools;
using TimeTrack.Web.Service.Tools.V1;
using TimeTrack.Web.Service.Validators;

namespace TimeTrack.Web.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TimeTrackDbContext>(x =>
            {
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddSingleton<JsonWebTokenConfigurationValidator>();
            services.AddSingleton<DatabaseConfigurationValidator>();
            
            services.Configure<JsonWebTokenConfiguration>(Configuration.GetSection("JwtOptions"));

            var jwtOptions = Configuration.GetSection("JwtOptions").Get<JsonWebTokenConfiguration>();

            services.AddScoped<ProjectUseCase>();
            services.AddScoped<CustomerUseCase>();
            services.AddScoped<ActivityTypeUseCase>();
            services.AddScoped<MemberUseCase>();
            services.AddScoped<ActivityUseCase>();
            services.AddScoped<OtherUseCase>();
            services.AddScoped<AccountUseCase>();

            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescription =>
                {
                    return apiDescription.First();
                });
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "TimeTrack API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Email = "jeavhletem@googlemail.com"
                    },
                    Description = "",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT"
                    }
                });
                var xmlFile = $"TimeTrack.Web.Service.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(AuthenticationSchemes.Cookie) 
                .AddCookie(AuthenticationSchemes.Cookie, options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                }).AddJwtBearer(AuthenticationSchemes.Bearer, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1) 
                    };
                });
            
            services.AddControllers();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="dbContext"></param>
        public void Configure(ILogger<Startup> logger, IApplicationBuilder app, IWebHostEnvironment env, TimeTrackDbContext dbContext)
        {
            dbContext.Setup();

            if (Configuration.GetValue<bool>("EnableCustomLogging"))
            {
                app.Use(async (context, next) =>
                {
                    logger.LogInformation(
                        $"Path: {context.Request.Path.Value} Method: {context.Request.HttpContext.Request.Method}"+
                        $" Protocol: {context.Request.Protocol} IP: {context.Request.HttpContext.Connection.RemoteIpAddress}"
                    );
                
                    await next.Invoke();
                });
            }
            
  
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/docs.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/docs/v1/docs.json", "TimeTrack API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles("/assets");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute("default", "{controller=Activity}/{action=Index}");
            });
        }
    }
}
