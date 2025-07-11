using Domain.Abstractions;
using Domain.Companies;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Companies;

public sealed class CompanyGetAllQuery() : IRequest<IQueryable<CompanyGetAllQuerResponse>>;

public sealed class CompanyGetAllQuerResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public new string CreateUserName { get; set; } = default!;
    public new string? UpdateUserName { get; set; }
}

internal sealed class CompanyGetAllQueryHandler(
    ICompanyRepository companyRepository,
    UserManager<AppUser> userManager) : IRequestHandler<CompanyGetAllQuery, IQueryable<CompanyGetAllQuerResponse>>
{
    public Task<IQueryable<CompanyGetAllQuerResponse>> Handle(CompanyGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from company in companyRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on company.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on company.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new CompanyGetAllQuerResponse
                        {
                            Name = company.Name,
                            City = company.City,
                            Email = company.Email,
                            Address = company.Address,
                            Phone = company.Phone,
                            
                            // Audit properties with user information
                            Id = company.Id,
                            IsActive = company.IsActive,
                            CreateAt = company.CreateAt,
                            CreateUserId = company.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateAt = company.UpdateAt,
                            UpdateUserId = company.UpdateUserId,
                            UpdateUserName = company.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                            IsDeleted = company.IsDeleted,
                            DeleteAt = company.DeleteAt
                        });
        
        return Task.FromResult(response);
    }
}

