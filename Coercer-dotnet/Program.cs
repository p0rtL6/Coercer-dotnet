using Coercer_dotnet.methods;
using Coercer_dotnet.structures;
using Coercer_dotnet.utils;

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

            HashSet<string> targets = (options.TargetOptions.TargetIps.Value?.Addresses) ?? throw new Exception("Targets list is null.");
            foreach (string target in targets)
            {
                string? listenerIp = null;

                switch (options.Mode)
                {
                    case Mode.COERCE:
                        listenerIp = options.ListenerOptions.ListenerIp.Value?.ToString();
                        break;
                    case Mode.SCAN:
                        listenerIp = NetworkUtils.GetIpAddressToListenOn(options, target)?.ToString();
                        break;
                    case Mode.FUZZ:
                        listenerIp = NetworkUtils.GetIpAddressToListenOn(options, target)?.ToString();
                        break;
                }

                listenerIp = listenerIp ?? throw new Exception("Listener IP cannot be null.");
                Method[] methods = availableMethods.Instantiate(options.AdvancedOptions.AuthType.Value, listenerIp, options.AdvancedOptions.HttpPort.Value, options.AdvancedOptions.SmbPort.Value);
            }

            Logger.Log('+', "All done! Bye Bye!");
        }
    }
}