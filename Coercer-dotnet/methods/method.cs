namespace Coercer_dotnet.methods
{
    public class Method
    {
        public string[] ExploitPaths { get; }
        public NcanNpAccess NcanNpAccess { get; }
        public NcacnIpTcpAccess NcacnIpTcpAccess { get; }
        public Protocol Protocol { get; }

        public Method(string[] exploitPaths, NcanNpAccess ncanNpAccess, NcacnIpTcpAccess ncacnIpTcpAccess, Protocol protocol)
        {
            ExploitPaths = exploitPaths;
            NcanNpAccess = ncanNpAccess;
            NcacnIpTcpAccess = ncacnIpTcpAccess;
            Protocol = protocol;
        }

        public void Trigger()
        {

        }
    }

    public class Access
    {
        public string Uuid { get; }
        public string Version { get; }

        public Access(string uuid, string version)
        {
            Uuid = uuid;
            Version = version;
        }
    }

    public class NcanNpAccess : Access
    {
        public string NamedPipe { get; }
        public NcanNpAccess(string namedPipe, string uuid, string version) : base(uuid, version)
        {
            NamedPipe = namedPipe;
        }
    }

    public class NcacnIpTcpAccess : Access
    {
        public NcacnIpTcpAccess(string uuid, string version) : base(uuid, version) { }
    }

    public class Protocol
    {
        public string Name { get; }
        public string ShortName { get; }
        public Protocol(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }
    }

    public class Function
    {
        public string Name { get; }
        public int Opnum { get; }
        public string[] VulnerableArguments { get; }

        public Function(string name, int opnum, string[] vulnerableArguments)
        {
            Name = name;
            Opnum = opnum;
            VulnerableArguments = vulnerableArguments;
        }
    }
}