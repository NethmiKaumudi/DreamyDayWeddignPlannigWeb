using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using FluentValidation.AspNetCore;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using DreamyDayWeddingPlanningWeb.Models.Validators;
using FluentValidation;
using DreamyDayWeddingPlanningWeb.Business.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register EmailSender as IEmailSender
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<WeddignTaskValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<WeddingValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GuestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BudgetValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TimelineEventValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VendorValidator>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeddingTaskService, WeddingTaskService>();
builder.Services.AddScoped<IWeddingService, WeddingService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();




var app = builder.Build();
// 👇 Add this after app.Build()
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Planner", "Couple" };

    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
