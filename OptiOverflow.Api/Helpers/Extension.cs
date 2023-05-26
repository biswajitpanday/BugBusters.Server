﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OptiOverflow.Core.Entities;
using OptiOverflow.Core.Interfaces.Repositories;
using OptiOverflow.Core.Interfaces.Services;
using OptiOverflow.Repository;
using OptiOverflow.Repository.Base;
using OptiOverflow.Repository.DatabaseContext;
using OptiOverflow.Service;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace OptiOverflow.Api.Helpers;

public static class Extension
{
    public static void ConfigureAppComponents(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog();
        builder.ConfigureAppSwagger();
        builder.ConfigureAppCorsPolicy();
        builder.ConfigureAppDbContext();
        builder.ConfigureAppIdentity();
        builder.ConfigureAppAuthentication();
        builder.ConfigureAppAutoMapper();
    }

    public static void ConfigureDI(this WebApplicationBuilder builder)
    {
        builder.ConfigureAppServices();
        builder.ConfigureAppRepositories();
    }


    #region App Components

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        try
        {
            const string logOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] \t{Message}{NewLine:1}{Exception:1}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: logOutputTemplate)
                .WriteTo.File(new MessageTemplateTextFormatter(outputTemplate: logOutputTemplate),
                    "Logs/log.txt",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    shared: true
                )
                .Enrich.WithProperty("ApplicationName", "OptiOverflow")
                .CreateLogger();
            builder.Host.UseSerilog();
            Log.Logger.Information("Serilog Configured Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up Serilog: {ex}");
        }
    }

    private static void ConfigureAppSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1",
                new OpenApiInfo
                { Title = "OptiOverflow", Version = "v1", Description = "OptiOverflow API" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }

    private static void ConfigureAppCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(option =>
            option.AddPolicy("AppPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
    }

    private static void ConfigureAppDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    private static void ConfigureAppIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddRoleValidator<RoleValidator<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureAppAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? string.Empty)),
                    ValidateIssuer = false,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = false,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                };
            });
    }

    private static void ConfigureAppAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(x => x.FullName!.StartsWith(nameof(OptiOverflow))));
    }


    #endregion


    #region App DI

    private static void ConfigureAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IUserProfileService, UserProfileService>();
        builder.Services.AddTransient<IQuestionService, QuestionService>();
        builder.Services.AddTransient<IAnswerService, AnswerService>();
        builder.Services.AddTransient<IVoteService, VoteService>();
    }

    private static void ConfigureAppRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        // todo: Add Repositories.
    }

    #endregion

}