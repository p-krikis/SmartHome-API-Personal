using SmartHomeAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Devices_Service>();
builder.Services.AddScoped<Users_Service>();
builder.Services.AddScoped<Houses_Service>();
builder.Services.AddScoped<Rooms_Service>();
builder.Services.AddScoped<Measurements_Service>();
builder.Services.AddScoped<Automations_Service>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
               builder =>
               {
                   builder.AllowAnyOrigin()
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
app.UseCors("AllowAllOrigins");
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
