using System.Text.RegularExpressions;

namespace Coercer_dotnet.utils
{
    public static class StringUtils
    {
        public static string CamelCaseToSpaceSeperated(string camelCaseString)
        {
            return (camelCaseString[..1] + string.Join(" ", Regex.Split(camelCaseString[1..], @"(?=[A-Z])"))).ToLower();
        }

        public static string ArrayToCommaSeperated(Array array)
        {
            return string.Join(", ", array.Cast<object>().Select(item => item?.ToString()));
        }

        public static string EnumToCommaSeperatedVariants<T>() where T : Enum
        {
            return string.Join(", ", Enum.GetNames(typeof(T)));
        }

        public static string EnumToCommaSeperatedVariants(Type enumType)
        {
            if (enumType.IsEnum)
            {
                return string.Join(", ", Enum.GetNames(enumType));
            }
            else
            {
                throw new Exception("Provided type is not an enum.");
            }
        }
    }

}