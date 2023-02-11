using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusalaDAL.Configuration;
using MusalaDAL.Context;
using MusalaDAL.Data;
using MusalaDAL.Service.Jobs;
using MusalaDAL.Services;
using MusalaDAL.Services.IServices;
using Quartz;
using Serilog;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace MusalaAPI
{
    public class Startup
    {
        public string ConnectionString { get; set; }
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _logger = Log.ForContext<Startup>();
            Configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddIdentity<ApplicationUser, ApplicationRole>(o => {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.SignIn.RequireConfirmedEmail = true;
                o.SignIn.RequireConfirmedPhoneNumber = true;
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));



            services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<SwaggerFileUploadFilter>();


                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Musala Interview API",
                    Description = "Musala Interview API"
                });
                // To Enable authorization using Swagger (JWT)
                /*swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer Token' [space] in the text input below.\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                });
                */
            });

            // configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // To immediately reject the access token
                };
            });

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // Create a "key" for the job
                var jobKey = new JobKey("HelloWorldJob");

                q.AddJob<DroneBatteryLevelQueryJob>(opts => opts.WithIdentity("DroneBatteryLevelQueryJob"));

                q.AddTrigger(opts => opts
                    .ForJob("DroneBatteryLevelQueryJob")
                    .WithIdentity("DroneBatteryLevelQueryJob-trigger")
                    .WithCronSchedule("0/30 * * * * ?"));
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);

            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<ICheckBatteryLevelService, CheckBatteryLevelService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("Development"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Musala Interview v1"));
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
