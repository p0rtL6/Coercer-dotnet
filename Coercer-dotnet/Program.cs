using Coercer_dotnet.structures;

namespace Coercer_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Options.ShowHelpMenu();
            }
            else
            {
                if (args[0] == "--help" || args[0] == "-h")
                {
                    Options.ShowHelpMenu();
                    return;
                }
                Mode mode = (Mode)Enum.Parse(typeof(Mode), args[0].ToUpper());
                Options options = new(mode, args);
            }
        }
    }
}