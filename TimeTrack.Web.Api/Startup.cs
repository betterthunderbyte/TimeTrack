using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TimeTrack.Core;
using TimeTrack.Core.Configuration;
using TimeTrack.Core.Configuration.Validators;
using TimeTrack.Core.UseCase;
using TimeTrack.UseCase;
using TimeTrack.Web.Api.Common;

namespace TimeTrack.Web.Api
{
    public class XmlSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (model.Properties == null) return;

            foreach (var entry in model.Properties)
            {
                var name = entry.Key;
                entry.Value.Xml = new OpenApiXml
                {
                    Name = name.Substring(0, 1).ToUpper() + name.Substring(1)
                };
            }
        }
    }
    
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
            services.AddDbContext<TimeTrackTimeTrackDbContext>(x =>
            {
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddScoped<ITimeTrackDbContext>(provider => provider.GetService<TimeTrackTimeTrackDbContext>());
            
            services.AddSingleton<JsonWebTokenConfigurationValidator>();

            services.Configure<JsonWebTokenConfiguration>(Configuration.GetSection("JwtOptions"));

            var jwtOptions = Configuration.GetSection("JwtOptions").Get<JsonWebTokenConfiguration>();
            
            services.AddScoped<IProjectUseCase, ProjectUseCase>();
            services.AddScoped<ICustomerUseCase, CustomerUseCase>();
            services.AddScoped<IActivityTypeUseCase, ActivityTypeUseCase>();
            services.AddScoped<IMemberUseCase, MemberUseCase>();
            services.AddScoped<IActivityUseCase, ActivityUseCase>();
            services.AddScoped<IOtherUseCase, OtherUseCase>();
            services.AddScoped<IAccountUseCase, AccountUseCase>();
            
            services.AddAuthentication(AuthenticationSchemes.Bearer) 
                .AddJwtBearer(AuthenticationSchemes.Bearer, options =>
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

            services.AddControllers().AddXmlSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<XmlSchemaFilter>();
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
                var xmlFile = $"TimeTrack.Web.Api.xml";
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
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TimeTrackTimeTrackDbContext timeTrackDbContext)
        {
            timeTrackDbContext.Setup();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "docs/{documentName}/docs.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/docs/v1/docs.json", "TimeTrack API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}