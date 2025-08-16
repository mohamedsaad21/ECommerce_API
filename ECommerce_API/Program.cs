using ECommerce.Application.Common;
using ECommerce.Application.Filters;
using ECommerce.Application.IServices;
using ECommerce.Application.Mappings;
using ECommerce.Application.Services;
using ECommerce.Application.Validators.Filters;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Configurations;
using ECommerce.Infrastructure.Persistence.Repository;
using ECommerce_API.Extensions;
using ECommerce_API.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ApiVersion = Asp.Versioning.ApiVersion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30",
        new CacheProfile
        {
            Duration = 30
        }
        );
    options.Filters.Add(new ExceptionHandlingFilter());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.AddSwaggerConfiguration();
builder.AddJWTConfiguration();

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(mapper => mapper.AddProfile<MappingProfile>());
builder.Services.AddScoped<ProductValidateAttribute>();
builder.Services.AddScoped<ShoppingCartValidateAttribute>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IFileService, FileService>();


builder.Services.AddResponseCaching();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
}).AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "StackStore V1");
        //options.RoutePrefix = string.Empty;
        options.RoutePrefix = "swagger";
    });
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { SD.Role_Admin, SD.Role_Company, SD.Role_Customer };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    //var FirstName = "Mohamed";
    //var LastName = "Saad";
    var Username = "adminuser";
    var Email = "admin@stackstore.com";
    var password = "Ad@123";

    var user = new ApplicationUser
    {
        //FirstName = FirstName,
        //LastName = LastName,
        UserName = Username,
        Email = Email,
    };
    await userManager.CreateAsync(user, password);
    await userManager.AddToRoleAsync(user, SD.Role_Admin);
}
app.Run();
