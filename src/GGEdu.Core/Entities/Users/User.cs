using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;

namespace GGEdu.Core.Entities.Users
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        public virtual string? SecurityStamp { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        // Gender:False mean 'Male' true mean 'Female'
        public bool Gender { get; set; }

        public string Photo { get; set; }

        public virtual Teacher? Teacher { get; set; }
        public virtual Student? Student { get; set; }
    }
}
