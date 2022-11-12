using System;
using System.Collections.Generic;

namespace DSM.Entities.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string? UserType1 { get; set; }

    public virtual ICollection<SignUp> SignUps { get; } = new List<SignUp>();
}
