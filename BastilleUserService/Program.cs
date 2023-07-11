using BastilleUserLibrary.Infrastructure.Extensions;
using BastilleUserService.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddContextAndConfiguration(configuration);
builder.Services.AddRegisteredServices();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddAuthenticationExtension(configuration);
builder.Services.AddAuthorizationExtension();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Bastille";
    options.Cookie.Path= "/";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});
var app = builder.Build();

//Seed the database
//SeedServiceExtension.Seed(app).GetAwaiter().GetResult();
SeedServiceExtension.IntializeAsync(app).GetAwaiter().GetResult();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        setupAction.SwaggerEndpoint("/swagger/BastilleOpenAPI/swagger.json", "Bastille API");
    });
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.SameAsRequest,
    MinimumSameSitePolicy = SameSiteMode.None
});
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
