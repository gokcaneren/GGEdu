using GGEdu.CompositionRoot.Enums;

namespace GGEdu.CompositionRoot.Attributes
{
    public class LifeTimeAttribute : Attribute
    {
        public ServiceLifetimeType LifetimeType{ get; set; }
        public LifeTimeAttribute(ServiceLifetimeType lifetimeType)
        {
            LifetimeType = lifetimeType;
        }
    }
}
