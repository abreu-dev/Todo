using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Todo.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<Board> Boards { get; set; }
    }
}
