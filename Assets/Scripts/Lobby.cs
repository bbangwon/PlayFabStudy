using Cysharp.Threading.Tasks;
using PlayFabStudy.Handlers;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
using System.Linq;
using UnityEngine;

namespace PlayFabStudy
{
    public class Lobby : MonoBehaviour
    {
        MatchmakingHandler quickMatchHandler;

        void Start()
        {
            if(ApplicationModel.CurrentPlayer == null)
            {
                var loginHandler = new LoginHandler();
                loginHandler.Login(OnPlayerLogin, OnLoginFail);
            }
        }

        #region Login

        private void OnPlayerLogin(PlayerInfo playerInfo)
        {
            Debug.Log("로그인 성공");
            ApplicationModel.CurrentPlayer = playerInfo;
        }

        private void OnLoginFail()
        {
            Debug.Log("로그인 실패");
        }

        #endregion Login

        public async void OnClickQuickMatch()
        {
            if (quickMatchHandler != null) return;

            quickMatchHandler = new MatchmakingHandler(ApplicationModel.CurrentPlayer, new MatchmakingQueueConfiguration { 
                QueueName = Constants.QUICK_MATCHMAKING_QUEUE_NAME,
                EscapeObject = false,
                ReturnMemberAttributes = true            
            });

            await quickMatchHandler.CreateTicket(new QuickMatchAttributes { Skill = "skill" });
            await quickMatchHandler.EnsureGetTicketStatus();

            Debug.Log(quickMatchHandler.Status);

            if(quickMatchHandler.Status == MatchmakingHandler.TicketStatus.Matched)
            {
                Debug.Log("매치완료");
                var matchResult = await quickMatchHandler.GetMatch();
                var orderedPlayers = matchResult?.Members?.OrderBy(member => member.Entity.Id).ToList();
                var playerOne = orderedPlayers?.ElementAtOrDefault(0) ?? null;
                var playerTwo = orderedPlayers?.ElementAtOrDefault(1) ?? null;

                Debug.Log(quickMatchHandler.MatchId);
                Debug.Log($"{playerOne.Entity.Id} {playerOne.Attributes.DataObject}");
                Debug.Log($"{playerTwo.Entity.Id} {playerTwo.Attributes.DataObject}");
            }
        }

        public void OnClickCancel()
        {
            quickMatchHandler.CancelPlayerTicket().Forget();
        }

    }
}
