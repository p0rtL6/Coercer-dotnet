using Coercer_dotnet.methods;
using Coercer_dotnet.structures;

namespace Coercer_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Title();

            AvailableMethods availableMethods = new();
            Options options = new(args);
            Logger logger = new(options);

            switch (options.Mode)
            {
                case Mode.COERCE:
                    break;
                case Mode.SCAN:
                    break;
                case Mode.FUZZ:
                    break;
            }

            Logger.Log('+', "All done! Bye Bye!");
        }
    }
}