//using DoctorConsult.Models;
//using DoctorConsult.Web.Helpers;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();





//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
//{
//    options.Password.RequiredLength = 6;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.User.RequireUniqueEmail = true;
//}).AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();




//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularApp",
//        builder =>
//        {
//            builder.AllowAnyOrigin() // Update with your Angular app's URL
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DCConn"), options =>
//    {
//        options.CommandTimeout(120);
//    })
//);

//builder.Services.AddControllers();
//builder.Services.AddApplicationServices();

//// Adding Authentication  
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//// Adding Jwt Bearer  
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["JWT:ValidAudience"],
//        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//    };
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHttpsRedirection();
//}








//app.UseStaticFiles();
//var env = app.Services.GetRequiredService<IWebHostEnvironment>();
//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "UploadedAttachments")),
//    RequestPath = "/UploadedAttachments",
//    EnableDirectoryBrowsing = true
//});




//app.UseRouting();
//app.UseCors("AllowAngularApp");
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

////Important for deploy
//app.MapFallbackToFile("/index.html");


////app.Use(async (context, next) =>
////{
////    await next();
////    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
////    {
////        context.Request.Path = "/index.html";
////        await next();
////    }
////});
using DoctorConsult.Models;
using DoctorConsult.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.AllowAnyOrigin() // Update with your Angular app's URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   ;
        });
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DCConn"), options =>
    {
        options.CommandTimeout(120);
    })
);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllers();
builder.Services.AddApplicationServices();

//// Adding Authentication  
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//// Adding Jwt Bearer  
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["JWT:ValidAudience"],
//        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//    };
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}




//app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");


app.UseStaticFiles();
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "UploadedAttachments")),
    RequestPath = "/UploadedAttachments",
    EnableDirectoryBrowsing = true
});

//Important for deploy
app.MapFallbackToFile("/index.html");




app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

app.Use(async (context, next) =>
{
    // Add X-Frame-Options header
    context.Response.Headers.Add("X-Frame-Options", "ALLOWALL"); // This allows content to be embedded anywhere

    await next();
    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
    {
        context.Request.Path = "/index.html";
        await next();
    }
});
