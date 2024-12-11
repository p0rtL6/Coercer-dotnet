using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Coercer_dotnet.utils
{
    public static class NetworkUtils
    {
        public static IPAddress? GetIpAddressToListenOn(Options options, string target)
        {
            IPAddress? listeningIp = null;
            if (options.ListenerOptions.IpAddress.Value is not null)
            {
                listeningIp = options.ListenerOptions.IpAddress.Value;
            }
            else if (options.ListenerOptions.InterfaceOption.Value is not null)
            {
                listeningIp = NetworkInterface.GetAllNetworkInterfaces()
                .Where(networkInterface => networkInterface.Name == options.ListenerOptions.InterfaceOption.Value)
                .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                .FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip.Address))?.Address;
                if (listeningIp is null)
                {
                    Logger.Log('!', $"Could not get IP address of interface '{options.ListenerOptions.InterfaceOption.Value}'");
                }
            }
            else
            {
                int[] possiblePorts = new[] { 445, 139, 88 };
                foreach (int possiblePort in possiblePorts)
                {
                    try
                    {
                        using UdpClient udpClient = new();
                        udpClient.Connect(target, possiblePort);
                        IPEndPoint endPoint = (IPEndPoint)(udpClient.Client.LocalEndPoint ?? throw new Exception("Endpoint is null."));
                        listeningIp = endPoint.Address;
                        break;
                    }
                    catch { }
                }
                if (listeningIp is null)
                {
                    Logger.Log('!', $"Could not detect interface with a route to target machine '{target}'");
                }
            }

            return listeningIp;
        }
    }
}