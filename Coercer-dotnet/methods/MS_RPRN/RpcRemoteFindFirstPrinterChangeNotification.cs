namespace Coercer_dotnet.methods.MS_RPRN
{
    public class RpcRemoteFindFirstPrinterChangeNotification : MS_RPRN
    {
        public override string Description => "Coercing a machine to authenticate using function RpcRemoteFindFirstPrinterChangeNotification (opnum 62) of [MS-RPRN]: Print System Remote Protocol (https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-rprn/b8b414d9-f1cd-4191-bb6b-87d09ab2fd83)";
        public override Function Function => new("RpcRemoteFindFirstPrinterChangeNotification", 62, new[] { "pszLocalMachine" });

        // public override void Trigger() { }
    }
}