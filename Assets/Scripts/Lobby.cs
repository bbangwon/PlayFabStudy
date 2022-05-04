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
        QuickAndPartyMatchmakingManager quickMatchHandler;

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

            quickMatchHandler = new QuickAndPartyMatchmakingManager(
                ApplicationModel.CurrentPlayer,
                Constants.QUICK_MATCHMAKING_QUEUE_NAME,
                Constants.PARTY_MATCHMAKING_QUEUE_NAME)
            {
                GiveUpAfterSeconds = Constants.GIVE_UP_AFTER_SECONDS,
                QuickMatchAttributes = new QuickMatchAttributes { Skill = "skill" }
            };

            await quickMatchHandler.BeginMatch();
        }

        public void OnClickCancel()
        {
            quickMatchHandler.Cancel();
        }

    }
}
