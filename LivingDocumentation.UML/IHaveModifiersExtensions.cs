namespace LivingDocumentation.Uml
{
    public static class IHaveModifiersExtensions
    { 
        public static char ToUmlVisibility(this IHaveModifiers modifiers)
        {
            if (modifiers.IsPublic)
            {
                return '+';
            }

            if (modifiers.IsInternal)
            {
                return '~';
            }

            if (modifiers.IsProtected)
            {
                return '#';
            }

            return '-';
        }
    }
}