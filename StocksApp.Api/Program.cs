using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StocksApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Add WPF App interaction service
builder.Services.AddSingleton<IWpfInteractionService, WpfInteractionService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHub<WpfInteractionHub>("/interactionhub");

app.Run();