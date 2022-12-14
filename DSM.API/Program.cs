using DSM.BAL.Service;
using DSM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using DSM.BAL.Abstraction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddNewtonsoftJson
    (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson
    (options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var connectionString = builder.Configuration.GetConnectionString("DBCon");
builder.Services.AddDbContext<DemoProjectContext>(option =>
option.UseSqlServer(connectionString)
);
// Add services to the container.
builder.Services.AddScoped<IUserInterface, UserService>();

builder.Services.AddScoped<IEncrypt, EncryptService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
