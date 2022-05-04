using System.Linq;
using Cysharp.Threading.Tasks;
using PlayFab.MultiplayerModels;
using PlayFabStudy.Handlers;
using PlayFabStudy.Models;
using UnityEngine;

namespace PlayFabStudy
{

    public class QuickAndPartyMatchmakingManager
    {
        public string QuickQueueName { get; private set; }
        public string PartyQueueName { get; private set; }

        MatchmakingHandler quickHandler;
        MatchmakingHandler partyHandler;

        public int GiveUpAfterSeconds { get; set; } = 120;
        public bool EscapeObject { get; set; } = false;

        public IMatchmakingPlayerAttributes QuickMatchAttributes { get; set; } = null;
        public PartyTicketAttributes PartyTicketAttributes { get; private set; } = null;

        public PlayerInfo Player { get; private set; }

        public  QuickAndPartyMatchmakingManager(PlayerInfo player, string quickQueueName, string partyQueueName)
        {
            this.Player = player;
            this.QuickQueueName = quickQueueName;
            this.PartyQueueName = partyQueueName;

            quickHandler = new MatchmakingHandler(this.Player, new Helpers.MatchmakingQueueConfiguration
            {
                 EscapeObject = this.EscapeObject,
                 QueueName = quickQueueName,
                 ReturnMemberAttributes = true,
                 GiveUpAfterSeconds = this.GiveUpAfterSeconds
            });

            partyHandler = new MatchmakingHandler(this.Player, new Helpers.MatchmakingQueueConfiguration
            {
                EscapeObject = this.EscapeObject,
                QueueName = partyQueueName,
                ReturnMemberAttributes = true,
                GiveUpAfterSeconds = this.GiveUpAfterSeconds
            });
        }

        public async UniTask BeginMatch()
        {
            await BeginQuickMatch();
            if (quickHandler.IsMatched)
            {
                string networkId = await PostQuickMatchMatched();
                await BeginPartyMatch(networkId);

                if(partyHandler.IsMatched)
                {
                    PostPartyMatched().Forget();
                }
            }
        }

        async UniTask BeginQuickMatch()
        {
            await quickHandler.CreateTicket(this.QuickMatchAttributes);
            await quickHandler.EnsureGetTicketStatus();

            Debug.Log(quickHandler.Status);
        }

        async UniTask<string> PostQuickMatchMatched()
        {
            Debug.Log("Quick 매치완료");
            var matchResult = await quickHandler.GetMatch();
            var hostPlayer = GetPlayer(matchResult);

            Debug.Log(quickHandler.MatchId);
            Debug.Log($"{hostPlayer.Entity.Id} {hostPlayer.Attributes.DataObject}");

            string networkId = string.Empty;
            if (this.Player.Entity.Id == hostPlayer.Entity.Id)
            {
                //Host Player인 경우
                //Party 네트워크를 생성하고 네트워크 ID를 가져온다.
                networkId = "NetworkId";
            }

            return networkId;
        }

        async UniTask BeginPartyMatch(string networkId)
        {
            await partyHandler.CreateTicket(new PartyTicketAttributes
            {
                PreviousMatchId = quickHandler.MatchId,
                NetworkId = networkId
            });
            await partyHandler.EnsureGetTicketStatus();
        }

        async UniTaskVoid PostPartyMatched()
        {
            Debug.Log("Party 매치완료");
            var matchResult = await partyHandler.GetMatch();
            var hostPlayer = GetPlayer(matchResult);

            Debug.Log($"매치ID = {partyHandler.MatchId}");
            Debug.Log($"{hostPlayer.Entity.Id} {hostPlayer.Attributes.DataObject}");

            var hostPlayerAttribute = JsonUtility.FromJson<PartyTicketAttributes>(hostPlayer.Attributes.DataObject.ToString());
            Debug.Log($"접속할 네트워크 ID = {hostPlayerAttribute.NetworkId}");

            if (this.Player.Entity.Id != hostPlayer.Entity.Id)
            {
                // host Player가 아닌 경우
                // Party Network 에 Join
            }
        }

        MatchmakingPlayerWithTeamAssignment GetPlayer(GetMatchResult matchResult, int index = 0)
        {
            var orderedPlayers = matchResult?.Members?.OrderBy(member => member.Entity.Id);
            if(index < 0 || orderedPlayers == null || orderedPlayers.Count() <= index)
            {
                return null;
            }

            var player = orderedPlayers?.ElementAtOrDefault(index) ?? null;
            return player;
        }

        public void Cancel()
        {
            if(quickHandler.Status != MatchmakingHandler.TicketStatus.NotStarted)
            {
                quickHandler.CancelPlayerTicket().Forget();
            }

            if(partyHandler.Status != MatchmakingHandler.TicketStatus.NotStarted)
            {
                partyHandler.CancelPlayerTicket().Forget();
            }
        }
    }
}
