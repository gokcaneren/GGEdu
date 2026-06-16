using System.ComponentModel;

namespace GGEdu.Core.Enums
{
    public enum LanguageLevel
    {
        [Description("Native")]
        Native = 0,

        [Description("Advanced")]
        Advanced = 1,

        [Description("Intermediate")]
        Intermediate = 2,

        [Description("Beginner")]
        Beginner = 3
    }
}
