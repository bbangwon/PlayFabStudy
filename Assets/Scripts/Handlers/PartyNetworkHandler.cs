using Cysharp.Threading.Tasks;
using PlayFab.Party;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
using System;
using UnityEngine;

namespace PlayFabStudy.Handlers
{
    public class PartyNetworkHandler
    {
        PlayFabMultiplayerManager Manager => PlayFabMultiplayerManager.Get();
        public string NetworkId { get; private set; } = null;
        public bool IsCreator { get; private set; } = false;

        UniTaskCompletionSource networkTaskCompletionSource = null;

        public event PlayFabMultiplayerManager.OnNetworkJoinedHandler OnNetworkJoined
        {
            add => Manager.OnNetworkJoined += value;
            remove => Manager.OnNetworkJoined -= value;
        }

        public event PlayFabMultiplayerManager.OnNetworkLeftHandler OnNetworkLeft
        {
            add => Manager.OnNetworkLeft += value;
            remove => Manager.OnNetworkLeft -= value;
        }

        public event PlayFabMultiplayerManager.OnDataMessageReceivedNoCopyHandler OnDataMessageNoCopyReceived
        {
            add => Manager.OnDataMessageNoCopyReceived += value;
            remove =>  Manager.OnDataMessageNoCopyReceived -= value; 
        }

        public PlayFabMultiplayerManagerState State => Manager.State;

        bool PrepareNetworkTCS()
        {
            if(networkTaskCompletionSource?.GetStatus(0) == UniTaskStatus.Pending)
            {
                return false;
            }

            networkTaskCompletionSource = new UniTaskCompletionSource();
            return true;
        }


        public PartyNetworkHandler()
        {
            Manager.OnNetworkJoined += Manager_OnNetworkJoined;            
        }

        public async UniTask CreateAndJoinToNetwork()
        {
            if (!PrepareNetworkTCS()) return;

            this.NetworkId = null;
            this.IsCreator = true;

            Manager.CreateAndJoinNetwork();
            Manager.LocalPlayer.IsMuted = true;
            
            await networkTaskCompletionSource.Task;
        }

        public async UniTask JoinNetwork(string networkId)
        {
            if (!PrepareNetworkTCS()) return;

            this.NetworkId = null;
            this.IsCreator = false;

            Manager.JoinNetwork(networkId);
            Manager.LocalPlayer.IsMuted = true;            

            await networkTaskCompletionSource.Task;
        }

        public async UniTask LeaveNetwork()
        {
            if (!PrepareNetworkTCS()) return;

            this.IsCreator = false;
            this.NetworkId = null;
            Manager.LeaveNetwork();

            await networkTaskCompletionSource.Task;
        }

        public void SendDataMessage<T>(PartyNetworkMessage<T> message)
        {
            PartyNetworkMessageHelper.BufferData(message, out var buffer, out _);
            Manager.SendDataMessage(buffer, Manager.RemotePlayers, DeliveryOption.BestEffort);
        }

        void Manager_OnNetworkJoined(object sender, string networkId)
        {
            Debug.Log($"OnNetworkJoined NetworkID : {networkId}");

            this.NetworkId = networkId;
            if(networkTaskCompletionSource != null)
            {
                networkTaskCompletionSource.TrySetResult();
                networkTaskCompletionSource = null;
            }

            Manager.OnNetworkLeft += Manager_OnNetworkLeft;
            Manager.OnNetworkJoined -= Manager_OnNetworkJoined;
        }

        void Manager_OnNetworkLeft(object sender, string networkId)
        {
            Debug.Log($"OnNetworkLeft NetworkID : {networkId}");

            this.IsCreator = false;
            this.NetworkId = null;

            Manager.OnNetworkLeft -= Manager_OnNetworkLeft;
        }
    } 
}
