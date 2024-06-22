using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Services.VehicleInsuranceServices;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.CustomerAuthServices;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.VehicleMakeServices;
using reservation_system_be.Services.VehicleTypeServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.VehicleServices;
using reservation_system_be.Services.VehicleLogServices;
using reservation_system_be.Services.VehicleMaintenanceServices;
using reservation_system_be.Services.AdditionalFeaturesServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.FeedbackReportService;
using reservation_system_be.Services.InvoiceService;
using reservation_system_be.Helper;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.FrontReservationServices;
using reservation_system_be.Services.AdminReservationServices;
using reservation_system_be.Services.VehicleUtilizationReportServices;
using reservation_system_be.Services.EmployeeAuthService;
using reservation_system_be.Services.StripeService;
using reservation_system_be.Services.AdminVehicleServices;
using reservation_system_be.Services.RevenueReportServices;
using reservation_system_be.Services.CustomerVehicleServices;
using reservation_system_be.Services.CusVsFeedService;
using reservation_system_be.Services.CusVsFeedServices;
using reservation_system_be.Services.ReservationStatusServices;
using reservation_system_be.Services.DashboardStatusServices;
using reservation_system_be.Services.PaymentService;
using Azure.Storage.Blobs;
using reservation_system_be.Services.FileServices;
using reservation_system_be.Services.NotificationServices;
using reservation_system_be.Services.VehicleFilterServices;
using reservation_system_be.Services.FeedbackServices;
using reservation_system_be.Services.InsuranceExpiryCheckService;
using reservation_system_be.Services.VehicleMaintenanceDueService;
using reservation_system_be.Services.CheckCustomerReservationConflictsServices;
using reservation_system_be.Services.NewFolder;
using Microsoft.OpenApi.Models;
using reservation_system_be.Services.AdminNotificationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Google;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Specify the issuer
            ValidAudience = builder.Configuration["Jwt:Audience"], // Specify the audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Specify the secret key
        };
    });



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Define the JWT Bearer security scheme
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key Authorization",
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header
    });

    // Make sure Swagger UI requires a Bearer token to access endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("customer"));
    options.AddPolicy("AdminAndStaffOnly", policy => policy.RequireRole("admin", "staff"));
});




builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter()));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));


//add connection azure blob
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add session services
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//Injection List
builder.Services.AddScoped<IVehicleMakeService, VehicleMakeService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
builder.Services.AddScoped<IVehicleInsuranceService, VehicleInsuranceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ICustomerReservationService, CustomerReservationService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<CustomerAuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleLogService, VehicleLogService>();
builder.Services.AddScoped<IVehicleMaintenanceService, VehicleMaintenanceService>();
builder.Services.AddScoped<IAdditionalFeaturesService, AdditionalFeaturesService>();
builder.Services.AddScoped<IVehicleModelService, VehicleModelService>();
builder.Services.AddScoped<IFeedbackReportService, FeedbackReportService>();
builder.Services.AddScoped<EmployeeAuthService>();
builder.Services.AddScoped<IAdminReservationService, AdminReservationService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<IAdminVehicleService, AdminVehicleService>();
builder.Services.AddScoped<IVehicleUtilizationReportService, VehicleUtilizationReportService>();
builder.Services.AddScoped<IRevenueReportService, RevenueReportService>();
builder.Services.AddScoped<ISalesChartService, SalesChartService>();
builder.Services.AddScoped<IReservationStatusService, ReservationStatusService>();
builder.Services.AddScoped<IDashboardStatusService, DashboardStatusService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IFrontVehicleService, FrontVehicleService>();
builder.Services.AddScoped<IFrontReservationServices, FrontReservationServices>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IVehicleFilterService, VehicleFilterService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAdminNotificationService, AdminNotificationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IExternalLoginService, ExternalLoginService>();


builder.Services.AddTransient<IEmailService, EmailService>();


// The insurance expiry check service
builder.Services.AddHostedService<InsuranceExpiryCheckService>();

// The maintenance due check service
builder.Services.AddHostedService<VehicleMaintenanceDueService>();

// The customer reservation conflict service
builder.Services.AddHostedService<CheckCustomerReservationConflictsService>();

// The reservation automatic cancellation service
builder.Services.AddHostedService<ReservationPendingService>();

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        c.RoutePrefix = "swagger"; // Set the UI at the app's /swagger URL
    });
}

app.UseSession();

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
    //.RequireAuthorization(); // Require authorization for all controllers and actions

app.Run();