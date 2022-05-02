using PlayFabStudy.Handlers;
using PlayFabStudy.Models;
using UnityEngine;

namespace PlayFabStudy
{
    public class Lobby : MonoBehaviour
    {        
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

        public void OnClickQuickMatch()
        {

        }

    }
}
