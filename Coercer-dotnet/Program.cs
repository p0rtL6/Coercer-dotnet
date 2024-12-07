using Coercer_dotnet.structures;

namespace Coercer_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Mode mode = Mode.NONE;
            try
            {
                mode = (Mode)Enum.Parse(typeof(Mode), args[0].ToUpper());
            }
            catch { }
            Options options = new(mode, args);
        }
    }
}