using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Teachers.Languages
{
    public class TeacherLanguage : BaseEntity
    {
        public LanguageLevel Level { get; set; }

        public Guid LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
