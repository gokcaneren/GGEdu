using GGEdu.Core.Entities.Users;
using GGEdu.Core.Repositories.Users;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }
    }
}
