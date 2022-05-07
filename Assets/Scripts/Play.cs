using Cysharp.Threading.Tasks;
using PlayFab.Party;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayFabStudy
{
    public class Play : MonoBehaviour
    {
        private void Awake()
        {
            ApplicationModel.PartyNetwork.OnNetworkLeft += PartyNetwork_OnNetworkLeft;
            ApplicationModel.PartyNetwork.OnDataMessageNoCopyReceived += PartyNetworkHandler_OnDataMessageNoCopyReceived;
        }

        public void OnClickLeaveNetwork()
        {
            if (ApplicationModel.PartyNetwork.State != PlayFabMultiplayerManagerState.ConnectedToNetwork)
                return;            
            
            ApplicationModel.PartyNetwork.LeaveNetwork().Forget();
        }

        private void PartyNetwork_OnNetworkLeft(object sender, string networkId)
        {
            ApplicationModel.PartyNetwork.OnNetworkLeft -= PartyNetwork_OnNetworkLeft;
            ApplicationModel.PartyNetwork.OnDataMessageNoCopyReceived -= PartyNetworkHandler_OnDataMessageNoCopyReceived;

            SceneManager.LoadScene("Lobby");
        }

        public void OnClickMessageSend()
        {
            ApplicationModel.PartyNetwork.SendDataMessage(
                new PartyNetworkMessage<string>
                {
                    MessageType = PartyNetworkMessageTypes.CustomMessage,
                    MessageData = "Hello World"
                });
        }

        private void PartyNetworkHandler_OnDataMessageNoCopyReceived(object sender, PlayFabPlayer from, IntPtr buffer, uint bufferSize)
        {
            var message = PartyNetworkMessageHelper.GetParsedDataFromBuffer<PartyNetworkMessage<string>>(buffer, bufferSize);
            Debug.Log($"OnReceiveMessage Type ({message.MessageType}), Message ({message.MessageData})");
        }
    }
}