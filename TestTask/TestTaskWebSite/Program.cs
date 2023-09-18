var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Setting the Default Files
app.UseDefaultFiles();

//Adding Static Files Middleware Component to serve the static files
app.UseStaticFiles();

app.Run();
