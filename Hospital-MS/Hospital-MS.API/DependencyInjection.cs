using Hangfire;
using Hospital_MS.Core._Data;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Auth;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Auth;
using Hospital_MS.Services.Common;
using Hospital_MS.Services.HMS;
using Hospital_MS.Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Hospital_MS.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = true;
            }).AddNewtonsoftJson();


            //var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddAuthConfig(configuration);

            services.AddBackgroundJobsConfig(configuration);

            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddSwaggerServices();
            services.AddHttpContextAccessor();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISQLHelper, SQLHelper>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IAdmissionService, AdmissionService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IWardService, WardService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBedService, BedService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IInsuranceCompanyService, InsuranceCompanyService>();
            services.AddScoped<IPatientHistoryService, PatientHistoryService>();
            services.AddScoped<IPatientAttachmentService, PatientAttachmentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IMedicalServiceService, MedicalServiceService>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IJobTypeService, JobTypeService>();
            services.AddScoped<IJobLevelService, JobLevelService>();


            return services;

        }

        private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //RecurringJob.AddOrUpdate<IAppointmentService>(
            //    recurringJobId: "ProcessBookingsDailyAtMidnight",
            //    methodCall: service => service.UpdateAppointmentsToCompletedAsync(),
            //    cronExpression: Cron.Daily(),
            //    options: new RecurringJobOptions
            //    {
            //        TimeZone = TimeZoneInfo.Local,
            //        // QueueName = "default"
            //    }
            //);


            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();


            var JwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings?.Key!)),
                    ValidIssuer = JwtSettings?.Issuer,
                    ValidAudience = JwtSettings?.Audience,

                };
            });


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                options.User.RequireUniqueEmail = true;
            });



            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hospital-MS API",
                    Description = "API documentation for Hospital-MS"
                });


                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = """
                    JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: "Bearer 12345abcdef"
                    """
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
