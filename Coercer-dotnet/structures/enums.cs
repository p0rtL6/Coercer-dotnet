namespace Coercer_dotnet.structures
{
    public enum AuthType
    {
        NONE,
        SMB,
        HTTP
    }

    public enum Mode
    {
        NONE,
        COERCE,
        SCAN,
        FUZZ
    }

    public enum TransportName
    {
        NONE,
        MSRPC,
        DCERPC
    }
}