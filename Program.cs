using Auth0.AspNetCore.Authentication;
using Auth0MvcDemo.Filters;
using Auth0MvcDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiForbiddenExceptionFilter>();
});
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuth0WebAppAuthentication(options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"]!;
        options.ClientId = builder.Configuration["Auth0:ClientId"]!;
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"]!;
    })
    .WithAccessToken(options =>
    {
        options.Audience = builder.Configuration["Auth0:Audience"]!;
    });

builder.Services.AddHttpClient<IBooksApiService, BooksApiService>(client =>
{
    var baseUrl = builder.Configuration["BooksApi:BaseUrl"]
        ?? throw new InvalidOperationException("BooksApi:BaseUrl no está configurado.");

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<IUsersApiService, UsersApiService>(client =>
{
    var baseUrl = builder.Configuration["UsersApi:BaseUrl"]
        ?? throw new InvalidOperationException("UsersApi:BaseUrl no está configurado.");

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
