using Coercer_dotnet.structures;

namespace Coercer_dotnet.methods.MS_EVEN
{
    public abstract class MS_EVEN : Method
    {
        protected MS_EVEN(AuthType authType, string listener) : base(authType, listener) { }
        protected MS_EVEN(AuthType authType, string listener, int port) : base(authType, listener, port) { }
        protected MS_EVEN(AuthType authType, string listener, int httpPort, int smbPort) : base(authType, listener, httpPort, smbPort) { }
    }
}