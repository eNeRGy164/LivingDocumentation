using PlantUml.Builder;

namespace LivingDocumentation.Uml
{
    public static class IHaveModifiersExtensions
    { 
        public static VisibilityModifier ToUmlVisibility(this IHaveModifiers modifiers)
        {
            if (modifiers.IsPublic())
            {
                return VisibilityModifier.Public;
            }

            if (modifiers.IsInternal())
            {
                return VisibilityModifier.PackagePrivate;
            }

            if (modifiers.IsProtected())
            {
                return VisibilityModifier.Protected;
            }

            if (modifiers.IsPrivate())
            {
                return VisibilityModifier.Private;
            }

            return VisibilityModifier.None;
        }
    }
}
