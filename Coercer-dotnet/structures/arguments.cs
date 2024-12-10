using System.Net;
using System.Numerics;

namespace Coercer_dotnet.structures
{
    public class Hash
    {
        public string Nt { get; }
        public string? Lm { get; }

        public static Hash Parse(string hash)
        {
            return new Hash(hash);
        }

        public Hash(string hash)
        {
            string[] hashParts = hash.Split(":");
            if (hashParts.Length > 1)
            {
                Lm = hashParts[0];
                Nt = hashParts[1];
            }
            else
            {
                Nt = hash;
            }
        }

        public Hash(string nt, string lm)
        {
            Nt = nt;
            Lm = lm;
        }
    }

    public class Targets
    {
        public HashSet<string> Addresses { get; }

        public static Targets Parse(string address)
        {
            return new Targets(address);
        }

        public static Targets Parse(string[] addresses)
        {
            return new Targets(addresses);
        }

        public Targets(HashSet<IPAddress> addresses)
        {
            Addresses = addresses.Select(s => s.ToString()).ToHashSet();
        }
        public Targets(string[] addresses) : this(addresses.ToHashSet()) { }
        public Targets(string address) : this(new HashSet<string> { address }) { }

        public Targets(HashSet<string> addresses)
        {
            Addresses = new();
            foreach (string address in addresses)
            {
                if (IPAddress.TryParse(address, out _))
                {
                    Addresses.Add(address);
                    continue;
                }

                if (IsCIDR(address))
                {
                    Addresses = Addresses.Concat(ExpandCIDR(GetStartingIp(address))).ToHashSet();
                    continue;
                }

                try
                {
                    Dns.GetHostEntry(address);
                    Addresses.Add(address);
                    continue;
                }
                catch { }

                try
                {
                    Uri uri = new(address);
                    if (uri.Host == "")
                    {
                        throw new Exception();
                    }

                    if (uri.IsDefaultPort)
                    {
                        Addresses.Add(uri.Host);
                    }
                    else
                    {
                        Addresses.Add($"{uri.Host}:{uri.Port}");
                    }
                    continue;
                }
                catch { }

                throw new Exception($"Invalid address: {address}, must be an IP, FQDN, or URL.");
            }
        }

        private static bool IsCIDR(string address)
        {
            string[] cidrParts = address.Split("/");
            if (cidrParts.Length == 2)
            {
                if (IPAddress.TryParse(cidrParts[0], out IPAddress? parsedIp) && int.TryParse(cidrParts[1], out int subnetMask))
                {
                    if (parsedIp is not null)
                    {
                        if (parsedIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return subnetMask > 0 && subnetMask <= 32;
                        }
                        else if (parsedIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            return subnetMask > 0 && subnetMask <= 128;
                        }
                    }
                }
            }
            return false;
        }

        private static string GetStartingIp(string address)
        {
            string[] cidrParts = address.Split("/");
            if (cidrParts.Length == 2)
            {
                return GetStartingIp(IPAddress.Parse(cidrParts[0]), int.Parse(cidrParts[1])).ToString() + "/" + cidrParts[1];
            }
            else
            {
                throw new Exception("Address is not valid CIDR notation.");
            }
        }
        private static IPAddress GetStartingIp(IPAddress address, int subnetMask)
        {
            byte[] addressBytes = address.GetAddressBytes();
            int maskMax;
            int numberOfBytes;
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                maskMax = 32;
                numberOfBytes = 4;
            }
            else if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                maskMax = 128;
                numberOfBytes = 16;
            }
            else
            {
                throw new Exception("Ip Address must be Ipv4 or Ipv6.");
            }
            int hostBits = maskMax - subnetMask;
            for (int i = numberOfBytes - 1; i >= 0; i--)
            {
                if (hostBits <= 0)
                    break;

                int bitsToZero = Math.Min(8, hostBits);
                addressBytes[i] &= (byte)(0xFF << bitsToZero);
                hostBits -= bitsToZero;
            }
            return new IPAddress(addressBytes);
        }

        private static HashSet<string> ExpandCIDR(string address)
        {
            string[] cidrParts = address.Split("/");
            if (cidrParts.Length == 2)
            {
                IPAddress cidrAddress = IPAddress.Parse(cidrParts[0]);
                int subnetMask = int.Parse(cidrParts[1]);
                return ExpandCIDR(cidrAddress, subnetMask);
            }
            else
            {
                throw new Exception("Invalid CIDR address.");
            }
        }
        private static HashSet<string> ExpandCIDR(IPAddress address, int subnetMask)
        {
            HashSet<string> addresses = new();

            {
                byte[] addressBytes = address.GetAddressBytes();

                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    int ipCount = (int)Math.Pow(2, 32 - subnetMask);
                    uint startIp = BitConverter.ToUInt32(addressBytes.Reverse().ToArray(), 0);

                    for (int i = 0; i < ipCount; i++)
                    {
                        uint currentIp = startIp + (uint)i;
                        byte[] currentIpBytes = BitConverter.GetBytes(currentIp);
                        Array.Reverse(currentIpBytes);
                        addresses.Add(new IPAddress(currentIpBytes).ToString());
                    }
                }
                else if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    BigInteger ipCount = BigInteger.Pow(2, 128 - subnetMask);
                    BigInteger startIp = new(addressBytes.Reverse().ToArray());

                    for (BigInteger i = 0; i < ipCount; i++)
                    {
                        var currentIpBytes = startIp + i;
                        byte[] currentIpByteArray = currentIpBytes.ToByteArray();

                        Array.Resize(ref currentIpByteArray, 16);
                        Array.Reverse(currentIpByteArray);

                        addresses.Add(new IPAddress(currentIpByteArray).ToString());
                    }
                }
                else
                {
                    throw new Exception("Ip must be either v4 or v6.");
                }
            }
            return addresses;
        }
    }
}