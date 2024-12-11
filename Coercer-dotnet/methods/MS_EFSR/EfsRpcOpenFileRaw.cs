using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcOpenFileRaw : MS_EFSR
    {
        public EfsRpcOpenFileRaw(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcOpenFileRaw(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcOpenFileRaw(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcOpenFileRaw", 0, new[] { "FileName" });

        // public override void Trigger() { }
    }
}