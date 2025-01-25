using FrameUp.ClientForm.Domain.Interfaces;
using FrameUp.ClientForm.Infra.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = MongoClientSettings.FromConnectionString("YourMongoDBConnectionString");// to do
    return new MongoClient(settings);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();


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
