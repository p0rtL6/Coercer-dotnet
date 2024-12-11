using Coercer_dotnet.methods;
using Coercer_dotnet.structures;

namespace Coercer_dotnet
{
    public class Logger
    {
        private static readonly string titleArt = @"   ______                                     ____        __             __ 
  / ____/___  ___  _____________  _____      / __ \____  / /_____  ___  / /_
 / /   / __ \/ _ \/ ___/ ___/ _ \/ ___/_____/ / / / __ \/ __/ __ \/ _ \/ __/
/ /___/ /_/ /  __/ /  / /__/  __/ /  /_____/ /_/ / /_/ / /_/ / / /  __/ /_    v0.1.0
\____/\____/\___/_/   \___/\___/_/        /_____/\____/\__/_/ /_/\___/\__/    by @p0rtL                                                                 
";

        private readonly Mode mode;
        private readonly bool verbose;
        private readonly List<LogEntry> logs;

        public Logger(Mode mode, bool verbose = false)
        {
            this.mode = mode;
            this.verbose = verbose;
            logs = new();
        }

        public Logger(Options options)
        {
            mode = options.Mode;
            verbose = options.Verbose.Value;
            logs = new();
        }

        public static void Title()
        {
            Console.WriteLine(titleArt);
        }

        public static void Log(char symbol, string message, ConsoleColor? color = null)
        {
            if (color is not null)
            {
                Console.Write("[");
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write(symbol);
                Console.ResetColor();
                Console.Write("] ");
                Console.Write(message);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"[{symbol}] {message}");
            }
        }

        public static void Testing(Method method, string path)
        {
            Console.Write("      [>] (");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("-testing-");
            Console.ResetColor();
            Console.Write(") ");
            method.Print(path);
            Console.WriteLine();
        }

        public static void Info(string message)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("info");
            Console.ResetColor();
            Console.Write("] ");
            Console.Write(message);
            Console.WriteLine();
        }

        public static void Warn(string message)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("warn");
            Console.ResetColor();
            Console.Write("] ");
            Console.Write(message);
            Console.WriteLine();
        }

        public static void Debug(string message)
        {
            Console.Write("[debug] ");
            Console.Write(message);
            Console.WriteLine();
        }

        private static void Result(char symbol, string message, ConsoleColor color, Method method, string path)
        {
            Console.Write("      [");
            Console.ForegroundColor = color;
            Console.Write(symbol);
            Console.ResetColor();
            Console.Write("] (");
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
            Console.Write(") ");
            method.Print(path);
            Console.WriteLine();
        }

        public void SaveResult(string target, Method method, Access access, string path, TestResult result)
        {
            logs.Add(new(target, method, access, path, result));
            if (mode == Mode.SCAN || mode == Mode.FUZZ)
            {
                if (result == TestResult.SMB_AUTH_RECEIVED)
                {
                    Result('+', "SMB Auth", ConsoleColor.Green, method, path);
                }
                else if (result == TestResult.HTTP_AUTH_RECEIVED)
                {
                    Result('+', "HTTP Auth", ConsoleColor.Green, method, path);
                }
                else if (result == TestResult.NCA_S_UNK_IF)
                {
                    Result('-', "-No Func-", ConsoleColor.Magenta, method, path);
                }
                else if (verbose)
                {
                    Result('!', result.ToString(), ConsoleColor.Red, method, path);
                }
            }
            else if (mode == Mode.COERCE)
            {
                if (result == TestResult.ERROR_BAD_NETPATH)
                {
                    Result('+', "ERROR_BAD_NETPATH", ConsoleColor.Green, method, path);
                }
                else if (verbose)
                {
                    Result('!', result.ToString(), ConsoleColor.Red, method, path);
                }
            }
        }

        // TODO (ExportXlsx, ExportJson, ExportSQLITE)
    }

    public class LogEntry
    {
        public Access Access { get; }
        public Function Function { get; }
        public Protocol Protocol { get; }
        public TestResult TestResult { get; }
        public string Path { get; }
        public string Target { get; }

        public LogEntry(string target, Method method, Access access, string path, TestResult result)
        {
            Access = access;
            Function = method.Function;
            Protocol = method.Protocol;
            TestResult = result;
            Path = path;
            Target = target;
        }
    }
}