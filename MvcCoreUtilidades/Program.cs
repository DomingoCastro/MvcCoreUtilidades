using MvcCoreUtilidades.Helpers;
using MvcCoreUtilidades.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<HelperPathProvider>();
//ESTE OBJETO DEBE SER INDECTADO COMO Singleton, LO QUE QUIERE DECIR QUE SOLAMENTE HAGA UNA VEZ EL NEW
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
string azureKeys = builder.Configuration.GetConnectionString("AzureStorage");
builder.Services.AddTransient<ServiceStorageFiles>(x => new ServiceStorageFiles(azureKeys));
builder.Services.AddTransient<ServiceStorageBlobs>(x =>new ServiceStorageBlobs(azureKeys));
builder.Services.AddTransient<ServiceStorageTables>(x => new ServiceStorageTables(azureKeys));
string urlApiToken = builder.Configuration.GetValue<string>("UrlApi:ApiToken");
builder.Services.AddTransient<ServiceAzureAlumnos>(x => new ServiceAzureAlumnos(urlApiToken));
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
