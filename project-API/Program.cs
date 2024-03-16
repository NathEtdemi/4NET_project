using Microsoft.EntityFrameworkCore;
using project_API;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "DB\\Application.db");

builder.Services.AddDbContext<DbContext, ApplicationDbContext>(
    c => c.UseSqlite($"Data Source={dbPath}"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
