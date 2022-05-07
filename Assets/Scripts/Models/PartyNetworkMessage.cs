using System;

namespace PlayFabStudy.Models
{
    [Serializable]
    public class PartyNetworkMessage<T>
    {
        public PartyNetworkMessageTypes MessageType;
        public T MessageData;
    }
}