using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcQueryUsersOnFile : MS_EFSR
    {
        public EfsRpcQueryUsersOnFile(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcQueryUsersOnFile(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcQueryUsersOnFile(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcQueryUsersOnFile", 6, new[] { "FileName" });

        // public override void Trigger() { }
    }
}