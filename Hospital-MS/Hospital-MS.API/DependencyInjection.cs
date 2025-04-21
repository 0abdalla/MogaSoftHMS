using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Services.Auth;
using Hospital_MS.Core.Services.Common;
using Hospital_MS.Reposatories._Data;
using Hospital_MS.Reposatories.Repositories;
using Hospital_MS.Services;
using Hospital_MS.Services.Auth;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
            services.AddControllers();

            //var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            options.AddDefaultPolicy(
            builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            ));

            services.AddAuthConfig(configuration);

            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services
                .AddSwaggerServices();
            //.AddMapsterConfig()
            //.AddFluentValidationConfig();

            services.AddHttpContextAccessor(); // To access the current HttpContext in services

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
            services.AddScoped<IInsuranceService, InsuranceService>();

            services.AddScoped<IPatientHistoryService, PatientHistoryService>();
            services.AddScoped<IPatientAttachmentService, PatientAttachmentService>();
            services.AddScoped<IFileService, FileService>();

            return services;

        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));


            /// services.AddOptions<JwtOptions>()           ==>>       Adds the IOptions<JwtOptions> service (DI) container.
            /// BindConfiguration(JwtOptions.SectionName)   ==>>       Binds a specific section of the configuration file (appsettings.json) to the JwtOptions class.
            /// ValidateOnStart();                          ==>>       Validates the options at application startup instead of waiting until the options are first requested.

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
                    ValidAudience = JwtSettings?.Audience
                };
            });


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                //options.SignIn.RequireConfirmedEmail = true;
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

                // Define the Bearer auth scheme
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

                // Require Bearer token globally
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
