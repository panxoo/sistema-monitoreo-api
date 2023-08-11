using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MonitorApi;
using MonitorApi.Data;
using MonitorApi.Models.Setting;
using MonitorApi.Services;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

 
// Add services to the container.

string stringConnexion = builder.Configuration.GetConnectionString("prueba_apiContext");
string stringConnexionAuth = builder.Configuration.GetConnectionString("prueba_apiContext");

builder.Services.Configure<TokensSettings>(builder.Configuration.GetSection("Tokens"));

builder.Services.AddAntiforgery(options =>     options.HeaderName = "X-XSRF-TOKEN");

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(stringConnexion));
builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(stringConnexionAuth));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    //cfg.Audience = builder.Configuration["Tokens:Issuer"];
                    //cfg.Authority = builder.Configuration["Tokens:Issuer"];
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Tokens:Issuer"],
                        ValidAudience = builder.Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
                    };
                    cfg.SaveToken = true;
                });
                    //.AddCookie(options =>
                    //{
                    //    options.Cookie.Name = "pruebassss";
                    //    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    //    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    //   options.Cookie.IsEssential = true;
                    //});
//.AddCookie(options =>
//{
//    options.SlidingExpiration = true;
//    options.Cookie.HttpOnly = false;
//    });

//builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>


//builder.Services.AddDataProtection()
//.PersistKeysToFileSystem(new
//                            DirectoryInfo(persistKeysToFileSystemDirectory))
//        .SetApplicationName(applicationName);

//builder.Services.ConfigureApplicationCookie(options => {
//    options.Cookie.Name = "Token";
//    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
//    options.Cookie.Path = "/";
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.Cookie.Domain = "localhost";

//    options.Cookie.Path = "/";// it's necessary, because by default cookie Path for App1 will be /App1, and for App2 /App2.
//});




builder.Services.AddCookiePolicy(options =>
{
    // options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
     options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;


});


builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false; 
    
   
}).AddEntityFrameworkStores<UsersDbContext>()
 .AddDefaultTokenProviders();



//builder.Services.ConfigureApplicationCookie(o =>
//{
//    o.ExpireTimeSpan = TimeSpan.FromMinutes(2);
//    o.SlidingExpiration = true;

//});



builder.Services.AddCors(options =>
{

    options.AddPolicy("politica",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
           .AllowAnyHeader()
         //  .WithHeaders(HeaderNames.ContentType, "x-custom-header")
           .AllowCredentials()
           //.WithExposedHeaders("Set-Cookie")
           ;
            

        });
});


builder.Services.AddScoped<TokenService, TokenService>();
 

//builder.WebHost.ConfigureKestrel((context, options) =>
//{

//        var httpsConfig = context.Configuration.GetSection("Kestrel:Endpoints:Https");
//        options.ListenAnyIP(httpsConfig.GetValue<int>("Port"), listenOptions =>
//        {
//            listenOptions.UseHttps(httpsConfig.GetValue<string>("Certificate:Path"), httpsConfig.GetValue<string>("Certificate:Password"));
//        });
//    });



var app = builder.Build();



//app.UseCors(builder =>

//    builder.SetIsOriginAllowed(origin => true)
//           .AllowAnyMethod()
//           .AllowAnyHeader()
//           .AllowCredentials()
//);





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

//app.Use((context, next) =>
//{

//    var requestPath = context.Request.Path.Value;
//    //if (context.Request.Headers.ContainsKey("X-XSRF-TOKEN"))
//    //{
//    //    antiforgery.ValidateRequestAsync(context);
//    //}

//    var ss = requestPath;

// if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
//        || string.Equals(requestPath, "/index.html", StringComparison.OrdinalIgnoreCase))
//{
//    var tokens = antiforgery.GetAndStoreTokens(context);
//        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false, Path = "/", SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, Secure = true });
//   }
//    return next(context);
//});

app.UseAntiforgeryToken();


app.UseCors("politica");
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 

app.Run();
