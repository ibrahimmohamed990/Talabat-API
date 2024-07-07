﻿using Microsoft.OpenApi.Models;

namespace Store.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreAPI", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                
                options.AddSecurityDefinition("bearer", securityScheme);
                var securityRequirments = new OpenApiSecurityRequirement
                {
                    { securityScheme , new []{"bearer" } }
                };
                options.AddSecurityRequirement(securityRequirments);
            });

            return services;

        }
    }
}
