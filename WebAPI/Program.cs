using Scalar.AspNetCore;
using WebAPI.Installers;
using WebAPI.Middlewares;
using WebAPI.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInternalServices(builder.Configuration);
builder.Services.AddExternalServices();

var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();
ExtensionsMiddleware.CreateFirstUser(app);

app.AddMiddlewares();
app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();
app.RegisterRoutes();


app.Run();
