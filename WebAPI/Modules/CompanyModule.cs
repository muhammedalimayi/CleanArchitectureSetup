

using Application.Companies;
using Domain.Companies;
using MediatR;
using TS.Result;

namespace WebAPI.Modules;

public static class CompanyModule
{
    public static void RegisterCompanyRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/companies").WithTags("Companies").RequireAuthorization();

        group.MapPost(string.Empty,
          async (ISender sender, CompanyCreateCommand request, CancellationToken cancellatioNToken) =>
          {
              var response = await sender.Send(request, cancellatioNToken);
              return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
          })
          .Produces<Result<string>>();


        group.MapGet(string.Empty,
            async (ISender sender, Guid id, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(new CompanyGetQuery(id), cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<Company>>();

    }
}
