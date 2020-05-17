using System.Collections.Generic;
using ErkinStudy.Domain.Entities.Lessons;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserFolder> UserFolders { get; set; }
    }
}
