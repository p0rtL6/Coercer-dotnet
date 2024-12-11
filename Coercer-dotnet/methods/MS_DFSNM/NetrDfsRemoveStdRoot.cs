using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_DFSNM
{
    public class NetrDfsRemoveStdRoot : MS_DFSNM
    {
        public NetrDfsRemoveStdRoot(AuthType authType, string listener) : base(authType, listener) { }
        public NetrDfsRemoveStdRoot(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public NetrDfsRemoveStdRoot(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "Coercing a machine to authenticate using function NetrDfsRemoveStdRoot (opnum 13) of [MS-DFSNM]: Distributed File System (DFS): Namespace Management Protocol (https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-dfsnm/95a506a8-cae6-4c42-b19d-9c1ed1223979)";
        public override Function Function => new("NetrDfsRemoveStdRoot", 13, new[] { "ServerName" });

        // public override void Trigger() { }
    }
}