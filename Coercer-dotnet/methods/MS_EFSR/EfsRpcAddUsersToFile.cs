namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcAddUsersToFile : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcAddUsersToFile", 9, new[] { "FileName" });

        // public override void Trigger() { }
    }
}