using filemanagementapi.Context;
using filemanagementapi.Interfaces;
using filemanagementapi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options=>options.UseSqlServer
(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddTransient<IAuth,AuthServices>();
builder.Services.AddTransient<IFile,FileServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options=>options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
