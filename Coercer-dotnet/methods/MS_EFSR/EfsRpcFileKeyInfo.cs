namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcFileKeyInfo : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcFileKeyInfo", 12, new[] { "FileName" });

        // public override void Trigger() { }
    }
}