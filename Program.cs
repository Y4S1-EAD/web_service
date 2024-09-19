using web_service.Models;
using web_service.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MongoDB settings from appsettings.json
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register the OrderService as a singleton
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<UserService>();

// Add controllers to the services container
builder.Services.AddControllers();

// Optional: Register Swagger services for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: Configure CORS if accessing from a different domain (e.g., React frontend)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowReactApp",
//         policy => policy.WithOrigins("http://localhost:3000")
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });

var app = builder.Build();

// Configure the HTTP request pipeline.

// Use Swagger middleware if in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional: Use CORS policy
// app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
