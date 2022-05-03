using PlayFab.MultiplayerModels;
using System;

namespace PlayFabStudy.Models
{
    [Serializable]
    public class PartyTicketAttributes : IMatchmakingPlayerAttributes
    {
        public string PreviousMatchId;
        public string NetworkId;

        public MatchmakingPlayerAttributes GetPlayerAttributes()
        {
            return new MatchmakingPlayerAttributes
            {
                DataObject = new PartyTicketAttributes
                {
                    PreviousMatchId = this.PreviousMatchId,
                    NetworkId = this.NetworkId
                }
            };
        }
    }
}
