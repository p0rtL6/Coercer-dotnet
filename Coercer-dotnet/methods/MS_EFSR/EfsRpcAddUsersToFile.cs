using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcAddUsersToFile : MS_EFSR
    {
        public EfsRpcAddUsersToFile(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcAddUsersToFile(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcAddUsersToFile(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcAddUsersToFile", 9, new[] { "FileName" });

        // public override void Trigger() { }
    }
}