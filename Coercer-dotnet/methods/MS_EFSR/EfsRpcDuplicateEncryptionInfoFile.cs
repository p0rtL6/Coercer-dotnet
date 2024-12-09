namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcDuplicateEncryptionInfoFile : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcDuplicateEncryptionInfoFile", 12, new[] { "SrcFileName" });

        // public override void Trigger() { }
    }
}