namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcDecryptFileSrv : MS_EFSR
    {
        public override string Description => "Coercing a machine to authenticate using function EfsRpcDecryptFileSrv (opnum 5) of [MS-EFSR Protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-efsr/08796ba8-01c8-4872-9221-1000ec2eff31)";
        public override Function Function => new("EfsRpcDecryptFileSrv", 5, new[] { "FileName" });

        // public override void Trigger() { }
    }
}