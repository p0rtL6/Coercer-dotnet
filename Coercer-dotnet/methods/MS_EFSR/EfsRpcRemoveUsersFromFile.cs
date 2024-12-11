using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcRemoveUsersFromFile : MS_EFSR
    {
        public EfsRpcRemoveUsersFromFile(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcRemoveUsersFromFile(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcRemoveUsersFromFile(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcRemoveUsersFromFile", 8, new[] { "FileName" });

        // public override void Trigger() { }
    }
}