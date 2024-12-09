namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcEncryptFileSrv : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcEncryptFileSrv", 4, new[] { "FileName" });

        // public override void Trigger() { }
    }
}