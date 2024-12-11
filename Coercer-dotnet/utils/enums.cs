namespace Coercer_dotnet.utils
{
    public static class EnumUtils
    {
        public static T ParseFromStringContainingVariantName<T>(string containingString, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) where T : Enum
        {
            string[] enumNames = Enum.GetNames(typeof(T));
            foreach (string enumName in enumNames)
            {
                if (containingString.Contains(enumName, stringComparison))
                {
                    return (T)Enum.Parse(typeof(T), enumName);
                }
            }
            throw new Exception("Enum name not contained within provided string.");
        }
    }
}