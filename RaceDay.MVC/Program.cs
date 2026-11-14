using RaceDay.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Enable HttpContextAccessor (so ApiService can access sessions)
builder.Services.AddHttpContextAccessor();

// 2. Configure HttpClient for the API
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5225/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // This is crucial: it allows the MVC app to store the API's session cookies
    UseCookies = true,
    CookieContainer = new System.Net.CookieContainer()
});

// 3. Enable Sessions for the MVC app (to remember who is logged in)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Add Session Middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();