using System;
using UnityEngine;
using PlayFabStudy.Models;
using PlayFab;
using PlayFab.ClientModels;

namespace PlayFabStudy.Handlers
{
    public class LoginHandler
    {
        public void Login(Action<PlayerInfo> OnSuccess, Action OnError)
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = this.GetPlayerCustomId(),
                CreateAccount = true
            };

            PlayFabClientAPI.LoginWithCustomID(request, result =>
            {
                var currentPlayer = new PlayerInfo
                {
                    EntityToken = result.EntityToken.EntityToken,
                    PlayFabId = result.PlayFabId,
                    SessionTicket = result.SessionTicket,
                    Entity = new PlayFab.AuthenticationModels.EntityKey
                    {
                        Id = result.EntityToken.Entity.Id,
                        Type = result.EntityToken.Entity.Type
                    }
                };

                OnSuccess(currentPlayer);

            }, 
            error => {
                Debug.LogError("PlayFab 로그인에 실패했습니다.");
                Debug.LogError($"응답코드: {error.HttpCode}");
                Debug.LogError($"디테일 에러: {error.ErrorDetails}");
                Debug.LogError($"에러 메시지: {error.ErrorMessage}");

                OnError();
            });
        }

        private string GetPlayerCustomId()
        {
            if(!PlayerPrefs.HasKey(Constants.PLAYFAB_PLAYER_CUSTOM_ID))
            {
                var newId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                PlayerPrefs.SetString(Constants.PLAYFAB_PLAYER_CUSTOM_ID, newId);
            }
            return PlayerPrefs.GetString(Constants.PLAYFAB_PLAYER_CUSTOM_ID);
        }
}

}