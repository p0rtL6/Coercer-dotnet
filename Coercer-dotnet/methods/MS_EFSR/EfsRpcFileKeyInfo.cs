using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcFileKeyInfo : MS_EFSR
    {
        public EfsRpcFileKeyInfo(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcFileKeyInfo(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcFileKeyInfo(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcFileKeyInfo", 12, new[] { "FileName" });

        // public override void Trigger() { }
    }
}