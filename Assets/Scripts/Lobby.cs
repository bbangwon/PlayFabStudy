using Cysharp.Threading.Tasks;
using PlayFabStudy.Handlers;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
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

            Debug.Log("종료");

        }

        public void OnClickCancel()
        {
            quickMatchHandler.CancelPlayerTicket().Forget();
        }

    }
}
