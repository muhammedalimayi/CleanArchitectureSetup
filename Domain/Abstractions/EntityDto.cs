namespace Domain.Abstractions;
public class EntityDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public string CreateUserName { get; set; } = default!;
    public DateTime UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeleteAt { get; set; }
}
