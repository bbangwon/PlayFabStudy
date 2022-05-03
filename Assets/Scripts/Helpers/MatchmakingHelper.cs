using PlayFab.MultiplayerModels;
using PlayFabStudy.Models;

namespace PlayFabStudy.Helpers
{
    public class MatchmakingHelper
    {
        public static MatchmakingPlayer GetMatchmakingPlayer(PlayerInfo player, IMatchmakingPlayerAttributes attributes)
        {
            return new MatchmakingPlayer
            {
                Attributes = attributes?.GetPlayerAttributes(),
                Entity = new EntityKey
                {
                    Id = player.Entity.Id,
                    Type = player.Entity.Type
                }
            };
        }
    }
}