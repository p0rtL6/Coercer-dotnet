namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcQueryUsersOnFile : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcQueryUsersOnFile", 6, new[] { "FileName" });

        // public override void Trigger() { }
    }
}