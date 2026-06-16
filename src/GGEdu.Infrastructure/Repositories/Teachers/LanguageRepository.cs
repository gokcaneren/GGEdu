using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }
    }
}
