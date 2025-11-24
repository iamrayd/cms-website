using project_cms.services;
using ProjectCms.Api.Services;
using ProjectCms.Models;
using ProjectCms.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

// 3. Register Services
builder.Services.AddSingleton<PageService>();
builder.Services.AddSingleton<PostService>();
builder.Services.AddSingleton<BannerService>();
builder.Services.AddSingleton<UserService>(); // ? NEW: User Service
builder.Services.AddSingleton<IActivityLogService, ActivityLogService>();
builder.Services.AddSingleton<ArchivedBannerService>();

// 4. Background Services
builder.Services.AddHostedService<BannerExpiryWorker>();

// 5. CORS for Angular dev (http://localhost:4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins
                (
                "http://localhost:4200",
                "http://localhost:7090", 
                "https://localhost:7090"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// 6. Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. Pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();