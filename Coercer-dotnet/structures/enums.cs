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

    public enum TransportNameFilter
    {
        NONE,
        MSRPC,
        DCERPC
    }

    public enum TestResult
    {
        NO_AUTH_RECEIVED = 0x0,
        SMB_AUTH_RECEIVED = 0x1,
        HTTP_AUTH_RECEIVED = 0x2,
        SMB_AUTH_RECEIVED_NTLMv1 = 0x3,
        SMB_AUTH_RECEIVED_NTLMv2 = 0x4,

        NCA_S_UNK_IF = 0x10001,

        ERROR_BAD_NETPATH = 0x35,
        ERROR_INVALID_NAME = 0x7b,

        RPC_X_BAD_STUB_DATA = 0x20001,
        RPC_S_ACCESS_DENIED = 0x5,
        RPC_S_INVALID_BINDING = 0x6a6,
        RPC_S_INVALID_NET_ADDR = 0x6ab,

        SMB_STATUS_PIPE_DISCONNECTED = 0x30001
    }
}