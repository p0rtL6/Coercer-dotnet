using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcDuplicateEncryptionInfoFile : MS_EFSR
    {
        public EfsRpcDuplicateEncryptionInfoFile(AuthType authType, string listener) : base(authType, listener) { }
        public EfsRpcDuplicateEncryptionInfoFile(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public EfsRpcDuplicateEncryptionInfoFile(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "";
        public override Function Function => new("EfsRpcDuplicateEncryptionInfoFile", 12, new[] { "SrcFileName" });

        // public override void Trigger() { }
    }
}