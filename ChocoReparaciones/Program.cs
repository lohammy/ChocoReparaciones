using ChocoReparaciones.Components;
using ChocoReparaciones.Components.Account;
using ChocoReparaciones.Data;
using ChocoReparaciones.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddScoped<ProductoServices>();
builder.Services.AddScoped<ReparacionServices>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;

	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	options.User.RequireUniqueEmail = true;

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();


using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
	var logger = services.GetRequiredService<ILogger<Program>>();

	try
	{
		string[] roles = { "Admin", "Cajero", "Tecnico" };

		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
				logger.LogInformation("Rol {Role} creado exitosamente.", role);
			}
		}

		string adminEmail = "admin@chocoreparaciones.com";
		string adminPassword = "Admin123!";
		string adminName = "Administrador";

		if (await userManager.FindByEmailAsync(adminEmail) is null)
		{
			var admin = new ApplicationUser
			{
				UserName = adminEmail,
				Email = adminEmail,
				EmailConfirmed = true

			};

			var result = await userManager.CreateAsync(admin, adminPassword);

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(admin, "Admin");
				logger.LogInformation("Usuario administrador creado: {Email}", adminEmail);
				logger.LogInformation("Credenciales por defecto - Email: {Email}, Password: {Password}", adminEmail, adminPassword);
			}
			else
			{
				logger.LogError("Error al crear usuario administrador: {Errors}",
					string.Join(", ", result.Errors.Select(x => x.Description)));
			}
		}
		else
		{
			logger.LogInformation("Usuario administrador ya existe: {Email}", adminEmail);
		}
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Error durante la inicialización de roles y usuario administrador");
	}
}

app.Run();
