using Application;
using Infrastructure;
using WebAPI.Middlewares;

namespace WebAPI.Installers;

public static class InternalServiceInstaller
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddCors();
        services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
        return services;
    }
}
