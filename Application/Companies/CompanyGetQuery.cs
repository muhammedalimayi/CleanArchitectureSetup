using Domain.Companies;
using MediatR;
using TS.Result;

namespace Application.Companies;
public sealed record CompanyGetQuery(
    Guid Id) : IRequest<Result<Company>>;

internal sealed class CompanyGetQueryHandler(
    ICompanyRepository companyRepository) : IRequestHandler<CompanyGetQuery, Result<Company>>
{
    public async Task<Result<Company>> Handle(CompanyGetQuery request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (company is null)
        {
            return Result<Company>.Failure("Firma bulunamadı");
        }

        return company;
    }
}