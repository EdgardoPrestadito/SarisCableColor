using ApiTicketRobChat.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//crear variable para la cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("Connection");

//registrar servicion para la conexion
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(connectionString, sqlServerOptions =>
//    {
//        sqlServerOptions.EnableRetryOnFailure();
//        //sqlServerOptions.TrustServerCertificate(true); // Ignorar problemas de certificado SSL
//    })
//);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
