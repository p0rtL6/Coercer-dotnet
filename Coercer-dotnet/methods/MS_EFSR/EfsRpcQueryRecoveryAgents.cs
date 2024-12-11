using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcQueryRecoveryAgents : MS_EFSR
    {
        public EfsRpcQueryRecoveryAgents(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcQueryRecoveryAgents(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcQueryRecoveryAgents(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcQueryRecoveryAgents", 7, new[] { "FileName" });

        // public override void Trigger() { }
    }
}