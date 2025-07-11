using Domain.Companies;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace Application.Companies;

public sealed record CompanyCreateCommand(string Name,
    string City,
    string Email,
    string Address,
    string Phone) : IRequest<Result<string>>;

public sealed class CompanyCreateCommandValidator : AbstractValidator<CompanyCreateCommand>
{
    public CompanyCreateCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(3).WithMessage("İsim alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.City).MinimumLength(3).WithMessage("Şehir alanı en az 3 karakter olmalıdır.");
        RuleFor(x => x.City).MinimumLength(9).MaximumLength(9).WithMessage("Geçerli bir telefon numarası yazınız.");
    }
}
internal sealed class CompanyCreateCommandHander(
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CompanyCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
    {
        var isCompanyExists = await companyRepository.AnyAsync(c => c.Name == request.Name);

        if (isCompanyExists)
        {
            return Result<string>.Failure("Bu isimdeki firma daha önce kaydedilmiş.");
        }

        Company company = request.Adapt<Company>();

        companyRepository.Add(company);
        await unitOfWork.SaveChangesAsync();

        return "Firma kaydı başarıyla tamamlandı.";
    }
}

