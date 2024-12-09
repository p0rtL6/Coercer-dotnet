namespace Coercer_dotnet.methods.MS_FSRVP
{
    public class IsPathSupported : MS_FSRVP
    {
        public override string Description => "Coercing a machine to authenticate using function IsPathSupported (opnum 8) of [MS-FSRVP Protocol](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fsrvp/dae107ec-8198-4778-a950-faa7edad125b)";
        public override Function Function => new("IsPathSupported", 8, new[] { "ShareName" });

        // public override void Trigger() { }
    }
}