using Microsoft.OpenApi.Models;
using TestIDFApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sample IDF app, better tested with: https://ifconfig.me/ip",
            Version = "v1",
            Description = "Try to emulate IDF basic clients"
        });
    });

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddTransient<ICrawl, Crawl>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();