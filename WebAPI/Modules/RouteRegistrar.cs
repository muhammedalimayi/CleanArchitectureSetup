namespace WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterCompanyRoutes();
        app.RegisterAuthRoutes();
    }
}