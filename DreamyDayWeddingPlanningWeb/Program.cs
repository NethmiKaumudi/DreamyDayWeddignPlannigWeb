using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
<<<<<<< Updated upstream
using FluentValidation.AspNetCore;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using DreamyDayWeddingPlanningWeb.Models.Validators;
using FluentValidation;
using DreamyDayWeddingPlanningWeb.Business.Services;
=======
using DreamyDayWeddingPlanningWeb.Services;

>>>>>>> Stashed changes
var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// ✅ Configure Services
// ------------------------------------------------------

<<<<<<< Updated upstream
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
=======
// 🔌 Database: MySQL with EF Core
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection")
    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
>>>>>>> Stashed changes

// 👤 Identity with Roles
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// 📄 PDF & View Rendering Services
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

// 🌐 MVC + Razor Pages
builder.Services.AddControllersWithViews();
<<<<<<< Updated upstream
builder.Services.AddScoped<IWeddingTaskService, WeddingTaskService>();
builder.Services.AddScoped<IWeddingService, WeddingService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IWeddingTimeLineService, WeddingTimeLineService>();




=======
builder.Services.AddRazorPages();
>>>>>>> Stashed changes

// ------------------------------------------------------
// ✅ Build App
// ------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------
// ✅ Seed Roles
// ------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Planner", "Couple" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// ------------------------------------------------------
// ✅ Middleware Pipeline
// ------------------------------------------------------
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

// ------------------------------------------------------
// ✅ Routing
// ------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Planner}/{action=Dashboard}/{id?}");

app.MapRazorPages(); // Required for Identity UI

// ------------------------------------------------------
// ✅ Run Application
// ------------------------------------------------------
app.Run();
