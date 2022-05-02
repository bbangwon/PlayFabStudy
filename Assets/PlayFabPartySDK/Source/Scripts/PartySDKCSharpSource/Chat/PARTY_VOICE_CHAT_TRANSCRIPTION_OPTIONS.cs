using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS
    //{
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_NONE = 0x0000,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_SELF = 0x0001,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_OTHER_CHAT_CONTROLS_WITH_MATCHING_LANGUAGES = 0x0002,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_OTHER_CHAT_CONTROLS_WITH_NON_MATCHING_LANGUAGES = 0x0004,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_DISABLE_HYPOTHESIS_PHRASES = 0x0008,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSLATE_TO_LOCAL_LANGUAGE = 0x0010,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_DISABLE_PROFANITY_MASKING = 0x0020,
        // PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_SELF_REGARDLESS_OF_NETWORK_STATE = 0x0040,
    //} PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS;
    public enum PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS : UInt32
    {
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_NONE = 0x0000,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_SELF = 0x0001,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_OTHER_CHAT_CONTROLS_WITH_MATCHING_LANGUAGES = 0x0002,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_OTHER_CHAT_CONTROLS_WITH_NON_MATCHING_LANGUAGES = 0x0004,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_DISABLE_HYPOTHESIS_PHRASES = 0x0008,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSLATE_TO_LOCAL_LANGUAGE = 0x0010,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_DISABLE_PROFANITY_MASKING = 0x0020,
        PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_TRANSCRIBE_SELF_REGARDLESS_OF_NETWORK_STATE = 0x0040,
    }
}