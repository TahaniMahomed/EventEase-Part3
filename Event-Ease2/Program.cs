/* S!--CODE ATTRIBUTION-->
<!--TITLE: (CLDV6211 POE Document)-->
<!--AUTHOR: (The Independent Institute of Education / Varsity College)-->
<!--DATE: (13 April 2026)-->
<!--VERSION: (POE Assignment Document)-->
<!--AVAILABLE: (https://advtechonline.sharepoint.com/:w:/r/sites/TertiaryStudents/_layouts/15/Doc.aspx?sourcedoc=%7B50C308BE-32AB-485D-83BA-81B7AF8B57CB%7D&file=CLDV6211POE.docx&action=default&mobileredirect=true)-->
*/
/* S!--CODE ATTRIBUTION
TITLE: Connecting to Azurite with Connection Strings
AUTHOR: Microsoft Documentation Team
DATE: 7 May 2026
VERSION: Azurite Emulator
AVAILABLE: https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite#connect-to-azurite-with-connection-strings
*/

/* S!--CODE ATTRIBUTION
TITLE: Dependency Injection in ASP.NET Core
AUTHOR: Microsoft ASP.NET Core Team
DATE: 7 May 2026
VERSION: ASP.NET Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
*/

/* S!--CODE ATTRIBUTION
TITLE: Configuration in ASP.NET Core (GetConnectionString)
AUTHOR: Microsoft ASP.NET Core Team
DATE: 7 May 2026
VERSION: ASP.NET Core 8.0
AVAILABLE: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#getconnectionstring
*/
/* S!--CODE ATTRIBUTION
TITLE: Visual Studio Guide: Using Azurite for Local Storage Development
AUTHOR: Visual Studio Tutorials (YouTube Channel)
DATE: 7 May 2026
VERSION: Video Tutorial
AVAILABLE: https://www.youtube.com/watch?v=0_45O6UozYw
*/

using Event_Ease2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// 1. Register your custom BlobService for Dependency Injection
builder.Services.AddScoped<Event_Ease2.Services.BlobService>();

// 2. Add standard MVC services
builder.Services.AddControllersWithViews();

// 3. Configure the SQL Server Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. CLEANED AZURE CLIENTS REGISTRATION
// This ensures the app uses "UseDevelopmentStorage=true" from appsettings.json
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));
});

var app = builder.Build();

// 5. Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

// Updated for .NET 9 features if applicable, otherwise standard static file support
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();