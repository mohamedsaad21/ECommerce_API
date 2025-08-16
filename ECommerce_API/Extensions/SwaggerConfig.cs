using Microsoft.OpenApi.Models;

namespace ECommerce_API.Extensions
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Stack Store",
                    Description = "Stack Store is a RESTful e-commerce API that manages products, categories, orders, carts, and user authentication.",
                    TermsOfService = new Uri("https://www.linkedin.com/in/mohamedsaad14/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mohamed Saad",
                        Email = "mohamedsaad2756@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/mohamedsaad14/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "My license",
                        Url = new Uri("https://www.linkedin.com/in/mohamedsaad14/")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your api key"
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
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}