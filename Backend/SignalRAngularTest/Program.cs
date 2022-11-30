using SignalRAngularTest.Hubs;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Cors to Container
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
//Adding SignalR to services container

builder.Services.AddSignalR();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// using CorsPolicy ( Must be included above Authentication and Authorization Middleware )
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

//Adding ChatHub Mapping
app.MapHub<ChatHub>("/Messages");

app.Run();
