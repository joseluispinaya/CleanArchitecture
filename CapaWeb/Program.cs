using CapaApplication.Interfaces;
using CapaApplication.Services;
using CapaInfrastucture.DataBase;
using CapaInfrastucture.Repositories;
using CapaWeb.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DbConnectionFactory(connectionString!));

builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
builder.Services.AddScoped<DirectorService>();

builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
