using Microsoft.OpenApi.Models;
using ChilluCodeCompilor.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configuration - consider adding environment variables
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Add this line

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with conditional API key requirements
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Compiler API", Version = "v1" });

    // Only add API key security if not in Development
    if (!builder.Environment.IsDevelopment())
    {
        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "X-API-KEY", // Must match your middleware
            Type = SecuritySchemeType.ApiKey,
            Description = "API key needed to access the endpoints"
        });

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
    }
});

var app = builder.Build();

// Enable Swagger based on configuration
var swaggerEnabled = app.Configuration.GetValue<bool>("EnableSwagger");
if (app.Environment.IsDevelopment() || swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Compiler API v1");

        // If you want to pre-fill the API key in development
        if (app.Environment.IsDevelopment())
        {
            c.ConfigObject.AdditionalItems["apiKey"] = "development-key";
        }
    });
}

app.UseHttpsRedirection();

// IMPORTANT: Uncomment this for production
// app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();

// Add this to ensure proper PORT handling on Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://*:{port}");