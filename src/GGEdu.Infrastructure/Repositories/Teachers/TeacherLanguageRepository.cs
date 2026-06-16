using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class TeacherLanguageRepository : GenericRepository<TeacherLanguage>, ITeacherLanguageRepository
    {
        public TeacherLanguageRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }
    }
}
