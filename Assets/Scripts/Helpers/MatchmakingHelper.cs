using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.MultiplayerModels;
using PlayFabStudy.Models;

namespace PlayFabStudy.Helpers
{
    public class MatchmakingHelper
    {
        public static MatchmakingPlayer GetMatchmakingPlayer(PlayerInfo player, MatchmakingPlayerAttributes attributes)
        {
            return new MatchmakingPlayer
            {
                Attributes = attributes,
                Entity = new EntityKey
                {
                    Id = player.Entity.Id,
                    Type = player.Entity.Type
                }
            };
        }
    }

}