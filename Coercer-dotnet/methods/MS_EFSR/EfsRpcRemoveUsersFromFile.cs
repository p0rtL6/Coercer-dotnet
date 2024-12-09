namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcRemoveUsersFromFile : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcRemoveUsersFromFile", 8, new[] { "FileName" });

        // public override void Trigger() { }
    }
}