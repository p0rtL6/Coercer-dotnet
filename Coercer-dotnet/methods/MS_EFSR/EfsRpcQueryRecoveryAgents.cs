namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcQueryRecoveryAgents : MS_EFSR
    {
        public override string Description => "";
        public override Function Function => new("EfsRpcQueryRecoveryAgents", 7, new[] { "FileName" });

        // public override void Trigger() { }
    }
}