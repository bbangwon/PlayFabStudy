using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_TEXT_CHAT_OPTIONS
    //{
        // PARTY_TEXT_CHAT_OPTIONS_NONE = 0x0000,
        // PARTY_TEXT_CHAT_OPTIONS_TRANSLATE_TO_LOCAL_LANGUAGE = 0x0001,
        // PARTY_TEXT_CHAT_OPTIONS_FILTER_OFFENSIVE_TEXT = 0x0002,
    //} PARTY_TEXT_CHAT_OPTIONS;
    public enum PARTY_TEXT_CHAT_OPTIONS : UInt32
    {
        PARTY_TEXT_CHAT_OPTIONS_NONE = 0x0000,
        PARTY_TEXT_CHAT_OPTIONS_TRANSLATE_TO_LOCAL_LANGUAGE = 0x0001,
        PARTY_TEXT_CHAT_OPTIONS_FILTER_OFFENSIVE_TEXT = 0x0002,
    }
}