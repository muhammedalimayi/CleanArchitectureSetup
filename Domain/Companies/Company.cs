using Domain.Abstractions;

namespace Domain.Companies;

public sealed class Company : Entity
{
    public string Name { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
}

