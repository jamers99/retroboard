using RetroBoard;
using RetroBoard.Azure;
using RetroBoard.Blazor.Demo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(builder.Configuration.GetSection("Azure").Get<AzureSettings>()!);

#if DEBUG
if (string.IsNullOrEmpty(builder.Configuration["Azure:Url"]))
    builder.Services.AddSingleton<IDataAccess, DemoDataAccess>();
else
#endif
    builder.Services.AddSingleton<IDataAccess, AzureDataAccess>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
