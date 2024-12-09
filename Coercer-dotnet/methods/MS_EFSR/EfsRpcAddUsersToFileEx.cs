namespace Coercer_dotnet.methods.MS_EFSR
{
    public class EfsRpcAddUsersToFileEx : MS_EFSR
    {
        public override string Description => "https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-efsr/d36df703-edc9-4482-87b7-d05c7783d65e";
        public override string Author => "";
        public override Function Function => new("EfsRpcAddUsersToFileEx", 15, new[] { "FileName" });

        // public override void Trigger() { }
    }
}