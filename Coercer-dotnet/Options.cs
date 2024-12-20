using System.Net;
using System.Reflection;
using Coercer_dotnet.structures;
using Coercer_dotnet.utils;

namespace Coercer_dotnet
{
    public abstract class OptionsBase
    {
        internal static bool ShownHelpMenu { get; set; } = false;
        internal static bool ShownProgramDescription { get; set; } = false;

        public Mode Mode { get; set; }

        public static void ShowProgramDescription()
        {
            ShownHelpMenu = true;
            ShownProgramDescription = true;
            Console.WriteLine(@"usage: coercer [-h] [-v] [--debug] {scan,coerce,fuzz} ...

Automatic windows authentication coercer using various methods.

positional arguments:
  {scan,coerce,fuzz}  Mode
    scan              Tests known methods with known working paths on all methods, and report when an authentication is received.
    coerce            Trigger authentications through all known methods with known working paths
    fuzz              Tests every method with a list of exploit paths, and report when an authentication is received.
");
        }

        private bool ArgumentInSelectedMode(Mode mode, PropertyInfo property)
        {
            Type propertyType = property.PropertyType;
            object? propertyValue = property.GetValue(this);

            var argumentModesProperty = propertyType.GetProperty("Modes");
            if (argumentModesProperty is not null)
            {
                object? argumentModes = argumentModesProperty.GetValue(propertyValue);
                if (argumentModes is not null)
                {
                    Mode[] argumentModesArray = (Mode[])argumentModes;
                    if (argumentModesArray.Contains(mode))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void ShowCategoryHelpMenu(bool exclusive, bool required)
        {
            if (!ShownProgramDescription)
            {
                ShowProgramDescription();
            }

            ShownHelpMenu = true;

            Type type = GetType();

            string categoryName = StringUtils.CamelCaseToSpaceSeperated(type.Name);

            Console.Write($"{categoryName}");

            if (exclusive)
            {
                Console.Write(" (exclusive)");
            }
            if (required)
            {
                Console.Write(" (required)");
            }
            Console.WriteLine(":");

            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                object? propertyValue = property.GetValue(this);

                if (propertyValue is not null)
                {
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Argument<>))
                    {
                        if (!ArgumentInSelectedMode(Mode, property))
                        {
                            continue;
                        }

                        Console.Write("  ");

                        var shortNameProperty = propertyType.GetProperty("ShortName");
                        if (shortNameProperty is not null)
                        {
                            var shortName = shortNameProperty.GetValue(propertyValue);
                            if (shortName is not null)
                            {
                                Console.Write($"{shortName}, ");
                            }
                        }

                        var nameProperty = propertyType.GetProperty("Name");
                        if (nameProperty is not null)
                        {
                            var name = nameProperty.GetValue(propertyValue);
                            Console.Write($"{name} ");
                        }

                        var genericType = propertyType.GetGenericArguments()[0];
                        if (genericType != typeof(bool))
                        {
                            Console.Write($"{genericType.Name}");
                        }

                        var requiredProperty = propertyType.GetProperty("Required");
                        if (requiredProperty is not null)
                        {
                            var requiredValue = requiredProperty.GetValue(propertyValue);
                            if (requiredValue is not null && (bool)requiredValue)
                            {
                                Console.Write(" (required)");
                            }
                        }

                        Console.WriteLine();
                        Console.Write("            ");

                        var descriptionProperty = propertyType.GetProperty("Description");
                        if (descriptionProperty is not null)
                        {
                            var description = descriptionProperty.GetValue(propertyValue);
                            if (description is not null && ((string)description).Length > 0)
                            {
                                Console.Write($"{description}");
                            }
                        }

                        var valueProperty = propertyType.GetProperty("Value");
                        if (valueProperty is not null)
                        {
                            var value = valueProperty.GetValue(propertyValue);
                            if (value is not null)
                            {
                                if (genericType != typeof(bool))
                                {
                                    if (genericType.IsArray)
                                    {
                                        string valueString = StringUtils.ArrayToCommaSeperated((Array)value);
                                        Console.Write($"(default: {valueString})");
                                    }
                                    else
                                    {
                                        Console.Write($" (default: {value})");
                                    }
                                }

                                if (genericType is not null && genericType.IsEnum)
                                {
                                    string enumString = StringUtils.EnumToCommaSeperatedVariants(genericType);
                                    Console.Write($" (choices: {enumString})");
                                }
                            }
                        }

                        Console.WriteLine();
                    }
                }
            }
        }
        public void Parse(string[] args, bool debug = false, bool verbose = false)
        {
            Mode = (Mode)Enum.Parse(typeof(Mode), args[0].ToUpper());

            Type type = GetType();

            bool exclusive = false;
            FieldInfo? exclusiveField = type.GetField("exclusive", BindingFlags.NonPublic | BindingFlags.Instance);
            if (exclusiveField is not null)
            {
                object? exclusiveValue = exclusiveField.GetValue(this);
                if (exclusiveValue is not null)
                {
                    exclusive = (bool)exclusiveValue;
                }
            }

            bool required = false;
            FieldInfo? requiredField = type.GetField("required", BindingFlags.NonPublic | BindingFlags.Instance);
            if (requiredField is not null)
            {
                object? requiredValue = requiredField.GetValue(this);
                if (requiredValue is not null)
                {
                    required = (bool)requiredValue;
                }
            }

            if (args.Contains("--help") || args.Contains("-h"))
            {
                ShowCategoryHelpMenu(exclusive, required);
                return;
            }

            int matchedArgsCount = 0;

            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                object? propertyValue = property.GetValue(this);

                if (propertyValue is not null)
                {
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Argument<>))
                    {

                        if (!ArgumentInSelectedMode(Mode, property))
                        {
                            continue;
                        }

                        int argIndex = -1;

                        var nameProperty = propertyType.GetProperty("Name");
                        if (nameProperty is not null)
                        {
                            var name = nameProperty.GetValue(propertyValue);
                            argIndex = Array.IndexOf(args, name);
                        }

                        var shortNameProperty = propertyType.GetProperty("ShortName");
                        if (shortNameProperty is not null)
                        {
                            var shortName = shortNameProperty.GetValue(propertyValue);
                            int shortNameIndex = Array.IndexOf(args, shortName);
                            if (argIndex == -1)
                            {
                                argIndex = shortNameIndex;
                            }
                        }

                        if (argIndex != -1)
                        {
                            matchedArgsCount++;

                            int valueIndex = argIndex + 1;
                            List<string> values = new();

                            while (valueIndex < args.Length)
                            {
                                if (args[valueIndex].StartsWith("-"))
                                {
                                    break;
                                }

                                values.Add(args[valueIndex]);
                                valueIndex++;
                            }

                            Type argumentValueType = propertyType.GetGenericArguments()[0];
                            var argumentValueProperty = propertyType.GetProperty("Value");
                            if (argumentValueProperty is not null)
                            {
                                if (argumentValueType.IsArray)
                                {
                                    Type? argumentElementType = argumentValueType.GetElementType();
                                    if (argumentElementType is not null)
                                    {
                                        if (argumentElementType == typeof(string))
                                        {
                                            argumentValueProperty.SetValue(propertyValue, values.ToArray());
                                        }
                                        else
                                        {
                                            MethodInfo? parseMethod = null;
                                            if (argumentElementType.IsEnum)
                                            {
                                                parseMethod = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string) });
                                            }
                                            else
                                            {
                                                parseMethod = argumentElementType.GetMethod("Parse", new[] { typeof(string), typeof(bool), typeof(bool) });
                                                parseMethod ??= argumentElementType.GetMethod("Parse", new[] { typeof(string), typeof(bool) });
                                                parseMethod ??= argumentElementType.GetMethod("Parse", new[] { typeof(string) });
                                            }
                                            if (parseMethod is not null)
                                            {
                                                var parsedListType = typeof(List<>).MakeGenericType(argumentElementType);
                                                var parsedList = Activator.CreateInstance(parsedListType);
                                                var addMethod = parsedListType.GetMethod("Add");

                                                if (addMethod is not null)
                                                {
                                                    foreach (string value in values)
                                                    {
                                                        object? parsedValue = null;
                                                        if (argumentElementType.IsEnum)
                                                        {
                                                            parsedValue = parseMethod.Invoke(null, new object[] { argumentElementType, value.ToUpper() });
                                                        }
                                                        else
                                                        {
                                                            ParameterInfo[] parameters = parseMethod.GetParameters();
                                                            int expectedParameterCount = parameters.Length;

                                                            if (expectedParameterCount == 1)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value });
                                                            }
                                                            else if (expectedParameterCount == 2)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value, debug });
                                                            }
                                                            else if (expectedParameterCount == 3)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value, debug, verbose });
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("No matching param count.");
                                                            }
                                                        }
                                                        if (parsedValue is not null)
                                                        {
                                                            addMethod.Invoke(parsedList, new object[] { Convert.ChangeType(parsedValue, argumentElementType) });
                                                        }
                                                    }
                                                }

                                                MethodInfo? toArrayMethod = parsedListType.GetMethod("ToArray");
                                                if (toArrayMethod is not null)
                                                {
                                                    object? arrayInstance = toArrayMethod.Invoke(parsedList, null);
                                                    if (arrayInstance is not null)
                                                    {
                                                        Array parsedArray = (Array)arrayInstance;
                                                        argumentValueProperty.SetValue(propertyValue, parsedArray);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception($"Argument {StringUtils.CamelCaseToSpaceSeperated(propertyType.Name)} does not have a parse method.");
                                            }
                                        }
                                    }
                                }
                                else if (argumentValueType.IsGenericType && argumentValueType.GetGenericTypeDefinition() == typeof(HashSet<>))
                                {
                                    Type? argumentElementType = argumentValueType.GetGenericArguments()[0];

                                    if (argumentElementType is not null)
                                    {
                                        if (argumentElementType == typeof(string))
                                        {
                                            argumentValueProperty.SetValue(propertyValue, values.ToHashSet());
                                        }
                                        else
                                        {
                                            MethodInfo? parseMethod = null;
                                            if (argumentElementType.IsEnum)
                                            {
                                                parseMethod = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string) });
                                            }
                                            else
                                            {
                                                parseMethod = argumentElementType.GetMethod("Parse", new[] { typeof(string), typeof(bool), typeof(bool) });
                                                parseMethod ??= argumentElementType.GetMethod("Parse", new[] { typeof(string), typeof(bool) });
                                                parseMethod ??= argumentElementType.GetMethod("Parse", new[] { typeof(string) });
                                            }
                                            if (parseMethod is not null)
                                            {
                                                var parsedHashsetType = typeof(HashSet<>).MakeGenericType(argumentElementType);
                                                var parsedHashset = Activator.CreateInstance(parsedHashsetType);
                                                var addMethod = parsedHashsetType.GetMethod("Add");

                                                if (addMethod is not null)
                                                {
                                                    foreach (string value in values)
                                                    {
                                                        object? parsedValue = null;
                                                        if (argumentElementType.IsEnum)
                                                        {
                                                            parsedValue = parseMethod.Invoke(null, new object[] { argumentElementType, value.ToUpper() });
                                                        }
                                                        else
                                                        {
                                                            ParameterInfo[] parameters = parseMethod.GetParameters();
                                                            int expectedParameterCount = parameters.Length;

                                                            if (expectedParameterCount == 1)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value });
                                                            }
                                                            else if (expectedParameterCount == 2)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value, debug });
                                                            }
                                                            else if (expectedParameterCount == 3)
                                                            {
                                                                parsedValue = parseMethod.Invoke(null, new object[] { value, debug, verbose });
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("No matching param count.");
                                                            }
                                                        }
                                                        if (parsedValue is not null)
                                                        {
                                                            addMethod.Invoke(parsedHashset, new object[] { Convert.ChangeType(parsedValue, argumentElementType) });
                                                        }
                                                    }
                                                }
                                                argumentValueProperty.SetValue(propertyValue, parsedHashset);
                                            }
                                            else
                                            {
                                                throw new Exception($"Argument {StringUtils.CamelCaseToSpaceSeperated(propertyType.Name)} does not have a parse method.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (values.Count == 0)
                                    {
                                        if (argumentValueType == typeof(bool))
                                        {
                                            argumentValueProperty.SetValue(propertyValue, true);
                                            continue;
                                        }
                                        else
                                        {
                                            throw new Exception($"Value is required for argument {StringUtils.CamelCaseToSpaceSeperated(property.Name)}.");
                                        }
                                    }

                                    if (argumentValueType == typeof(string))
                                    {
                                        if (values.Count == 1)
                                        {
                                            argumentValueProperty.SetValue(propertyValue, values[0]);
                                        }
                                        else
                                        {
                                            throw new Exception("Expected one argument for argument {StringUtils.CamelCaseToSpaceSeperated(property.Name)}, got multiple.");
                                        }
                                    }
                                    else
                                    {
                                        MethodInfo? parseMethod = null;
                                        object? toParse = null;
                                        if (argumentValueType.IsEnum)
                                        {
                                            values = values.Select(value => value.ToUpper()).ToList();
                                        }
                                        if (values.Count == 1)
                                        {
                                            parseMethod = argumentValueType.GetMethod("Parse", new[] { typeof(string), typeof(bool), typeof(bool) });
                                            parseMethod ??= argumentValueType.GetMethod("Parse", new[] { typeof(string), typeof(bool) });
                                            parseMethod ??= argumentValueType.GetMethod("Parse", new[] { typeof(string) });
                                            toParse = values[0].ToUpper();
                                        }
                                        else
                                        {
                                            parseMethod = argumentValueType.GetMethod("Parse", new[] { typeof(string[]), typeof(bool), typeof(bool) });
                                            parseMethod ??= argumentValueType.GetMethod("Parse", new[] { typeof(string[]), typeof(bool) });
                                            parseMethod ??= argumentValueType.GetMethod("Parse", new[] { typeof(string[]) });
                                            toParse = values.ToArray();
                                        }
                                        if (parseMethod is not null)
                                        {
                                            ParameterInfo[] parameters = parseMethod.GetParameters();
                                            int expectedParameterCount = parameters.Length;

                                            object? parsedValue = null;

                                            if (expectedParameterCount == 1)
                                            {
                                                parsedValue = parseMethod.Invoke(null, new object[] { toParse });
                                            }
                                            else if (expectedParameterCount == 2)
                                            {
                                                parsedValue = parseMethod.Invoke(null, new object[] { toParse, debug });
                                            }
                                            else if (expectedParameterCount == 3)
                                            {
                                                parsedValue = parseMethod.Invoke(null, new object[] { toParse, debug, verbose });
                                            }
                                            else
                                            {
                                                throw new Exception("No matching param count.");
                                            }
                                            argumentValueProperty.SetValue(propertyValue, Convert.ChangeType(parsedValue, argumentValueType));
                                        }
                                        else
                                        {
                                            throw new Exception($"Argument {StringUtils.CamelCaseToSpaceSeperated(propertyType.Name)} does not have a parse method.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (exclusive && matchedArgsCount > 1)
            {
                throw new Exception($"Found more than one argument in an exclusive category: {StringUtils.CamelCaseToSpaceSeperated(type.Name)}.");
            }

            if (required && matchedArgsCount == 0)
            {
                throw new Exception($"Category {StringUtils.CamelCaseToSpaceSeperated(type.Name)} requires an argument.");
            }
        }
    }

    public class Options : OptionsBase
    {
        private readonly bool exclusive = false;
        private readonly bool required = false;
        public Argument<bool> Help { get; }
        public Argument<bool> Verbose { get; }
        public Argument<bool> Debug { get; }
        public AdvancedOptions AdvancedOptions { get; }
        public FilterOptions FilterOptions { get; }
        public CredentialOptions CredentialOptions { get; }
        public TargetOptions TargetOptions { get; }
        public ListenerOptions ListenerOptions { get; }

        public Options(string[] args) : base()
        {
            Help = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--help", "-h", "Show the help menu", false, false);
            Verbose = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--verbose", "-v", "Verbose mode", false, false);
            Debug = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--debug", null, "Debug mode", false, false);

            if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
            {
                ShowProgramDescription();
                Environment.Exit(0);
            }

            Parse(args);

            if (!ShownHelpMenu)
            {
                Logger.Info($"Starting {Mode.ToString().ToLower()} mode");
            }

            AdvancedOptions = new(args);
            FilterOptions = new(args);
            CredentialOptions = new(args);
            TargetOptions = new(args, Debug.Value);
            ListenerOptions = new(args);

            if (ShownHelpMenu)
            {
                Environment.Exit(0);
            }
        }
    }

    public abstract class OptionsCategory : OptionsBase { }

    public class AdvancedOptions : OptionsCategory
    {
        private readonly bool exclusive = false;
        private readonly bool required = false;
        public Argument<string> ExportJson { get; }
        public Argument<string> ExportXlsx { get; }
        public Argument<string> ExportSqlite { get; }
        public Argument<int> Delay { get; }
        public Argument<int> MinHttpPort { get; }
        public Argument<int> MaxHttpPort { get; }
        public Argument<int> HttpPort { get; }
        public Argument<int> SmbPort { get; }
        public Argument<int> DcePort { get; }
        public Argument<int[]> DcePorts { get; }
        public Argument<AuthType> AuthType { get; }
        public Argument<bool> StopOnNtlmAuth { get; }
        public Argument<bool> AlwaysContinue { get; }

        public AdvancedOptions(string[] args) : base()
        {
            ExportJson = new(new[] { Mode.SCAN, Mode.FUZZ }, "--export-json", null, "Export results to specified JSON file", false);
            ExportXlsx = new(new[] { Mode.SCAN, Mode.FUZZ }, "--export-xlsx", null, "Export results to specified XLSX file", false);
            ExportSqlite = new(new[] { Mode.SCAN, Mode.FUZZ }, "--export-sqlite", null, "Export results to specified SQLITE3 database file", false);
            Delay = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--delay", null, "Delay between attempts (in seconds)", false);
            MinHttpPort = new(new[] { Mode.SCAN, Mode.FUZZ }, "--min-http-port", null, "Minimum HTTP port", false, 64000);
            MaxHttpPort = new(new[] { Mode.SCAN, Mode.FUZZ }, "--max-http-port", null, "Maximum HTTP port", false, 65000);
            HttpPort = new(new[] { Mode.COERCE, Mode.SCAN }, "--http-port", null, "HTTP port", false, 80);
            SmbPort = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--smb-port", null, "SMB port", false, 445);
            DcePort = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--dce-port", null, "DCERPC port", false, 135);
            DcePorts = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--dce-ports", null, "DCERPC ports", false);
            AuthType = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--auth-type", null, "Desired authentication type", false);
            StopOnNtlmAuth = new(new[] { Mode.SCAN }, "--stop-on-ntlm-auth", null, "Move on to next target on successful NTLM authentication", false, false);
            AlwaysContinue = new(new[] { Mode.COERCE }, "--always-continue", null, "Always continue to coerce", false, false);
            Parse(args);
        }
    }

    public class FilterOptions : OptionsCategory
    {
        private readonly bool exclusive = false;
        private readonly bool required = false;
        public Argument<string[]> FilterMethodName { get; }
        public Argument<string[]> FilterProtocolName { get; }
        public Argument<string[]> FilterPipeName { get; }
        public Argument<TransportNameFilter[]> FilterTransportName { get; }

        public FilterOptions(string[] args) : base()
        {
            FilterMethodName = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--filter-method-name", null, "", false);
            FilterProtocolName = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--filter-protocol-name", null, "", false);
            FilterPipeName = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--filter-pipe-name", null, "", false);
            FilterTransportName = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--filter-transport-name", null, "", false, new[] { TransportNameFilter.MSRPC, TransportNameFilter.DCERPC });
            Parse(args);
        }
    }

    public class CredentialOptions : OptionsCategory
    {
        private readonly bool exclusive = false;
        private readonly bool required = false;
        public Argument<string> Username { get; }
        public Argument<string> Password { get; }
        public Argument<string> Domain { get; }
        public Argument<Hash[]> Hashes { get; }
        public Argument<bool> NoPass { get; }
        public Argument<IPAddress> DcIp { get; }
        public Argument<bool> OnlyKnownExploitPaths { get; }

        public CredentialOptions(string[] args) : base()
        {
            Username = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--username", "-u", "Username to authenticate to the remote machine", false);
            Password = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--password", "-p", "Password to authenticate to the remote machine. (if omitted, it will be asked unless -no-pass is specified)", false);
            Domain = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--domain", "-d", "Windows domain name to authenticate to the machine", false);
            Hashes = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--hashes", null, "[LMHASH]:NTHASH - NT/LM hashes (LM hash can be empty)", false);
            NoPass = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--no-pass", null, "Don't ask for password (useful for -k)", false);
            DcIp = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--dc-ip", null, "IP Address of the domain controller. If omitted it will use the domain part (FQDN) specified in the target parameter", false);
            OnlyKnownExploitPaths = new(new[] { Mode.FUZZ }, "--only-known-exploit-paths", null, "Only test known exploit paths for each functions", false, false);
            Parse(args);

            if (!ShownHelpMenu && (Username.Value is null || Username.Value == ""))
            {
                Logger.Info("No credentials provided, trying to connect with a NULL session.");
            }

            if (!ShownHelpMenu && Password.Value is null && Username.Value is not null && (Hashes.Value is null || Hashes.Value.Length != 0) && NoPass.Value != true)
            {
                Console.Write("Password: ");
                string password = string.Empty;

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }

                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password = password[..^1];
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                }

                Console.WriteLine();
                Password.Value = password;
            }
        }
    }

    public class TargetOptions : OptionsCategory
    {
        private readonly bool exclusive = true;
        private readonly bool required = true;
        public Argument<Targets> TargetIps { get; }
        public Argument<string> TargetsFile { get; }

        public TargetOptions(string[] args, bool debug = false) : base()
        {
            TargetIps = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--target-ip", "-t", "IP address or hostname of the target machine", false);
            TargetsFile = new(new[] { Mode.COERCE, Mode.SCAN, Mode.FUZZ }, "--targets-file", "-f", "File containing a list of IP address or hostname of the target machines", false);
            Parse(args, debug);

            if (TargetsFile.Value is not null)
            {
                if (File.Exists(TargetsFile.Value))
                {
                    HashSet<string> targetIpsFromFile = File.ReadAllLines(TargetsFile.Value).ToHashSet();
                    if (TargetIps.Value is not null)
                    {
                        TargetIps.Value = new(TargetIps.Value.Addresses.Concat(targetIpsFromFile).ToHashSet(), debug);
                    }
                    else
                    {
                        TargetIps.Value = new(targetIpsFromFile, debug);
                    }
                }
                else
                {
                    throw new FileNotFoundException("Targets File path does not exist.");
                }
            }
        }
    }

    public class ListenerOptions : OptionsCategory
    {
        private readonly bool exclusive = true;
        private readonly bool required = false;
        public Argument<string> InterfaceOption { get; }
        public Argument<IPAddress> IpAddress { get; }
        public Argument<IPAddress> ListenerIp { get; }

        public ListenerOptions(string[] args) : base()
        {
            InterfaceOption = new(new[] { Mode.SCAN, Mode.FUZZ }, "--interface", "-i", "Interface to listen on incoming authentications", false);
            IpAddress = new(new[] { Mode.SCAN, Mode.FUZZ }, "--ip-address", "-I", "IP address to listen on incoming authentications", false);
            ListenerIp = new(new[] { Mode.COERCE }, "--listener-ip", "-l", "IP address or hostname of the listener machine", true);
            Parse(args);
        }
    }
}