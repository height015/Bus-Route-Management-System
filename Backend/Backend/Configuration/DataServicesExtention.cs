using System;
using System.Reflection;
using System.Text;
using AutoMapper;
using BRMSAPI.Data;
using BRMSAPI.Domain;
using BRMSAPI.Service;
using Core;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Contacts;
using Service.ServiceManager;

namespace BRMSAPI.Configuration;

public static class DataServicesExtention
{
    public static IServiceCollection AddExtDataServices(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddIdentity<Passengers, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.SignIn.RequireConfirmedEmail = true;

        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Token:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Token:JwtIssuer"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            }
        );
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole(AppConstant.AppUserAdmin));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole(AppConstant.AppUsers));
            options.AddPolicy("GuestPolicy", policy => policy.RequireRole(AppConstant.AppUsersGuest));
            options.AddPolicy("EditorPolicy", policy => policy.RequireRole(AppConstant.AppUsersEditor));
        });

        services.AddHttpClient();


        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IWebHelper, WebHelper>();

        services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

        services.AddScoped<IBusService, BusRepository>();

        services.AddScoped<IPickUpPointService, PickUpPointRepository>();

        services.AddScoped<ILocationService, LocationRepository>();

        services.AddScoped<IScheduleServices, ScheduleRepository>();

        services.AddScoped<ITokenService, TokenService>();

        //services.AddScoped<IWebHelper, WebHelper>();


        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo
        //    { Title = "Bus Route Management System Api (BRMS)",
        //        Version = "v1"
        //    }

        //    );
        //});

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Bus Route Management System Api (BRMS)",
                Description = "An ASP.NET Core Web API for Bus Route Management System " +
                " For Mapping Buses to Route",
                TermsOfService = null
                //Contact = new OpenApiContact
                //{
                //    Name = "Example Contact",
                //    Url = new Uri("test.com")
                //},
                //License = new OpenApiLicense
                //{
                //    Name = "Example License",
                //    Url = new Uri("test.com")
                //}
            });

            // using System.Reflection;
            //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });





        return services;
    }
}

