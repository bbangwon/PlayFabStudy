using PlayFab.MultiplayerModels;
using System;

namespace PlayFabStudy.Models
{
    [Serializable]
    public class QuickMatchAttributes : IMatchmakingPlayerAttributes
    {
        public string Skill;
        public MatchmakingPlayerAttributes GetPlayerAttributes()
        {
            return new MatchmakingPlayerAttributes
            {
                DataObject = new QuickMatchAttributes
                {
                    Skill = this.Skill
                }
            };
        }
    }
}
