using PlayFab.MultiplayerModels;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
using PlayFab;

namespace PlayFabStudy.Handlers
{
    public class MatchmakingHandler
    {
        PlayerInfo Player;

        public MatchmakingHandler(PlayerInfo player)
        {
            this.Player = player;
        }

        public void CreateTicket(string attribute, string networkId = "")
        {

        }

        CreateMatchmakingTicketRequest GetCreateRequest(MatchmakingPlayerAttributes attributes)
        {
            var request = new CreateMatchmakingTicketRequest
            {
                Creator = MatchmakingHelper.GetMatchmakingPlayer(this.Player, attributes),
                GiveUpAfterSeconds = Constants.GIVE_UP_AFTER_SECONDS,
                QueueName = Constants.QUICK_MATCHMAKING_QUEUE_NAME,
                AuthenticationContext = this.Player.PlayFabAuthenticationContext
            };

            return request;
        }
    }
}