namespace Coercer_dotnet.methods.MS_RPRN
{
    public class RpcRemoteFindFirstPrinterChangeNotificationEx : MS_RPRN
    {
        public override string Description => "Coercing a machine to authenticate using function RpcRemoteFindFirstPrinterChangeNotificationEx (opnum 65) of [MS-RPRN]: Print System Remote Protocol (https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-rprn/eb66b221-1c1f-4249-b8bc-c5befec2314d)";
        public override Function Function => new("RpcRemoteFindFirstPrinterChangeNotificationEx", 65, new[] { "pszLocalMachine" });

        // public override void Trigger() { }
    }
}