using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users;
public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}"; //computed property

    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTime CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTime UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}
