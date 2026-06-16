using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherLanguageOutputDto
    {
        public Guid Id { get; set; }
        public Guid LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public string LanguageLevelName { get; set; }
    }
}
