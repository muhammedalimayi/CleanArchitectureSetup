﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebAPI.Controller;

namespace WebAPI.Installers;

public static class ExternalServiceInstaller
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services
            .AddControllers()
            .AddOData(opt =>
            opt.Select()
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .SetMaxTop(null)
                .AddRouteComponents("odata", AppODataController.GetEdmModel()) // OData model binding added

        );
        services.AddRateLimiter(x =>
            x.AddFixedWindowLimiter("fixed", cfg =>
            {
                cfg.QueueLimit = 100;
                cfg.Window = TimeSpan.FromSeconds(1);
                cfg.PermitLimit = 100;
                cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            })
        );
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
        services.AddAuthorization();
        services.AddResponseCompression(opt =>
        {
            opt.EnableForHttps = true;
        });
        return services;
    }
}