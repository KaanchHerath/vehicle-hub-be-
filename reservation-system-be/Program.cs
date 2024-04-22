using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Services.VehicleInsuranceServices;
using reservation_system_be.Services.VehicleMakeServices;
using reservation_system_be.Services.VehicleTypeServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injection List
builder.Services.AddScoped<IVehicleMakeService, VehicleMakeService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
builder.Services.AddScoped<IVehicleInsuranceService, VehicleInsuranceService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Allow only this origin can change as per your frontend URL
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Use CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
