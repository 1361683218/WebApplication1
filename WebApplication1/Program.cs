using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = "persist security info=True;data source=rm-bp1tg219t7o5j5ex8fo.rwlb.rds.aliyuncs.com;port=3306;initial catalog=2109060214_db;user id=2109060214;password=2109060214;character set=utf8;allow zero datetime=true;convert zero datetime=true;pooling=true;maximumpoolsize=3000";
builder.Services.AddDbContext<_2109060214DbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34))));

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews(option =>
{
    option.Filters.Add(new WebApplication1.Filters.AuthFilter());
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
    pattern: "{controller=Home}/{action=ShowLogin}/{id?}");
app.UseSession();
app.Run();
