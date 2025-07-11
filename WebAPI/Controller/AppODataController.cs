using Application.Companies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace WebAPI.Controller;

[Route("odata")]
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<CompanyGetAllQuerResponse>("Companies");
        return builder.GetEdmModel();
    }

    [HttpGet("Companies")]
    public async Task<IQueryable<CompanyGetAllQuerResponse>> GetAllCompanies(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CompanyGetAllQuery(), cancellationToken);
        return response;
    }
}