using Domain.Companies;
using GenericRepository;
using Infrastructure.Context;

namespace Infrastructure.Repositories;
internal sealed class CompanyRepository : Repository<Company, ApplicationDbContext>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
