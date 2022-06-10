using IdentityApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                    //access to user manager
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //access to roles and managing it
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    //requirements for passwords 
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    //limit attempts to enter an app
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.AllowedForNewUsers = true;

    
    options.User.RequireUniqueEmail = true; //unique email
});

// Authentication - who are you, Authorization - what are you allowed to do
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

//Enabling Authorization Handlers
builder.Services.AddScoped<IAuthorizationHandler, InvoiceCreatorAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceManagerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceAdminAuthorizationHandler>();

var app = builder.Build();

//adds a prepaired user(manager,admin) and a role for it
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); //every time we start an app it will check if we
                                //have a db and if not create, if yes update it to latest ver.

    var seedUserPass = builder.Configuration.GetValue<string>("SeedUserPass");
    await SeedData.Initialize(services, seedUserPass); //and after db it will seed the users
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
