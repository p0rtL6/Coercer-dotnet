using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcEncryptFileSrv : MS_EFSR
    {
        public EfsRpcEncryptFileSrv(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcEncryptFileSrv(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcEncryptFileSrv(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcEncryptFileSrv", 4, new[] { "FileName" });

        // public override void Trigger() { }
    }
}