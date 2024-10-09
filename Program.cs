using web_service.Models;
using web_service.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Load the ENV package before creating the builder
if (File.Exists(".env"))
{
    DotNetEnv.Env.Load();
}

// Add services to the container.

// Configure MongoDB settings from appsettings.json or .env
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register the Service as a singleton
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<RatingService>();

// Add controllers and configure Newtonsoft.Json
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Optional: Register Swagger services for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: Configure CORS if accessing from a different domain (e.g., React frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// For all requests
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowReactApp",
//         policy => policy.WithOrigins("*")  // Allow all origins
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
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
