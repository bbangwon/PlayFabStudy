using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_OPTION
    //{
        // PARTY_OPTION_LOCAL_UDP_SOCKET_BIND_ADDRESS = 0,
        // PARTY_OPTION_LOCAL_DEVICE_DIRECT_PEER_CONNECTIVITY_OPTIONS_MASK = 1,
        // PARTY_OPTION_TEXT_CHAT_FILTER_LEVEL = 2,
    //} PARTY_OPTION;
    public enum PARTY_OPTION : UInt32
    {
        PARTY_OPTION_LOCAL_UDP_SOCKET_BIND_ADDRESS = 0,
        PARTY_OPTION_LOCAL_DEVICE_DIRECT_PEER_CONNECTIVITY_OPTIONS_MASK = 1,
        PARTY_OPTION_TEXT_CHAT_FILTER_LEVEL = 2,
    }
}