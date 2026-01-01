using BikeStoreWeb.Data.Context;
using BikeStoreWeb.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Scoped
builder.Services.AddApplicationServices();



builder.Services.AddDbContext<BikeStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwagger();
    app.UseSwagger();
    app.UseSwaggerUI(
        opt =>
        {
            opt.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            opt.RoutePrefix = string.Empty;
        }
    );
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

DbInitializer.Seed(app);

app.Run();


