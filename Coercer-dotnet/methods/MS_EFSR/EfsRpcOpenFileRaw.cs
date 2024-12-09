namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcOpenFileRaw : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcOpenFileRaw", 0, new[] { "FileName" });

        // public override void Trigger() { }
    }
}