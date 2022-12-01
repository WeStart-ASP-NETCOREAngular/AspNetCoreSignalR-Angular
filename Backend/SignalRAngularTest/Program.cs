using SignalRAngularTest.Hubs;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Step 1 enable cors to enable requests from Angular side ( or any js framework )
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


//Step 2 add SignalR to services container
builder.Services.AddSignalR();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Step 2 Use and add Cors from Service Container
// using CorsPolicy ( Must be included above Authentication and Authorization Middleware )

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.MapControllers();

//Adding ChatHub Mapping
// Step 2 add Custom Hubs to my App
app.MapHub<ChatHub>("/Messages");

app.Run();
