using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Data.Context;
using BikeStoreWeb.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// 1. SERVÝS TANIMLAMALARI
builder.Services.AddControllers();



// CORS Politikasý: React uygulamasýna (localhost:5173) izin ver
builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("AllowReactApp",
        b => b.WithOrigins("http://localhost:5173")    // React'ýn çalýþtýðý adres
        .AllowAnyMethod()
        .AllowAnyHeader());
    }
);

#region API VERSIONING AYARLARI
builder.Services.AddApiVersioning(options =>
{
    // API versiyon bilgisini response header'da gönder (api-supported-versions)
    options.ReportApiVersions = true;

    // Eðer istemci versiyon belirtmezse varsayýlan olarak 1.0 kabul et
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Varsayýlan versiyon
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Versiyonu URL'den okuyacaðýmýzý belirtiyoruz (api/v1/...)
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    // Swagger için versiyon formatý: 'v' + versiyon sayýsý (örn: v1)
    options.GroupNameFormat = "'v'VVV";

    // URL'deki versiyon parametresini otomatik doldur
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region JWT Configuration
//Identity Ayarlarý (Þifre kurallarý vb.)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;  //Email benzersiz olmalý
    opt.Password.RequireDigit = false;  //Basit þifreye izin ver (Dev için)
    opt.Password.RequiredLength = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
}).
AddEntityFrameworkStores<BikeStoreDbContext>()
.AddDefaultTokenProviders();
//JWT Authentication Ayarlarý
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(
    opt => opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    });

#endregion

// Scoped
builder.Services.AddApplicationServices();

builder.Services.AddDbContext<BikeStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id= "Bearer"
                    }
                },
                new string[] { }
            }
        });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeStore API v1");
            opt.RoutePrefix = string.Empty;
        }
    );
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

DbInitializer.Seed(app);

app.Run();


