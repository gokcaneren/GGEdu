using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Teachers.Input
{
    public class TeacherLanguageInputDto
    {
        public Guid LanguageId { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}
