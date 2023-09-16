using BuildTest.AppCore;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews()
.AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null)
.AddRazorOptions(opt =>
{
	opt.ViewLocationExpanders.Add(new ViewLocationExpander());

	//Area Locations
	opt.AreaViewLocationFormats.Clear();
	opt.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
	opt.AreaViewLocationFormats.Union(JAppConfigStore.CustomSharedDirectories());

});

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
