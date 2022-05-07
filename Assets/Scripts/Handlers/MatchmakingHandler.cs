using PlayFab.MultiplayerModels;
using PlayFabStudy.Helpers;
using PlayFabStudy.Models;
using PlayFab;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;

namespace PlayFabStudy.Handlers
{
    public class MatchmakingHandler
    {
        public enum TicketStatus
        {
            NotStarted,
            Matched,
            Canceled,            
            WaitingForMatch,
            WaitingForPlayers,
            WaitingForServer
        }

        PlayerInfo Player;

        public MatchmakingQueueConfiguration QueueConfiguration { get; set; }

        public CreateMatchmakingTicketResult MatchmakingTicketResult { get; set; }
        
        public GetMatchmakingTicketResult MatchmakingTicketStatus { get; set; }

        public GetMatchResult MatchResult { get; set; }

        public string TicketId => this.MatchmakingTicketResult?.TicketId ?? "";
        public string MatchId => this.MatchmakingTicketStatus?.MatchId ?? "";

        public TicketStatus Status
        {
            get
            {
                if (this.MatchmakingTicketStatus == null)
                    return TicketStatus.NotStarted;

                return (TicketStatus)Enum.Parse(typeof(TicketStatus), this.MatchmakingTicketStatus.Status);
            }
        }

        CancellationTokenSource cancellationTokenSource;

        public bool IsMatched => Status == TicketStatus.Matched;
        public bool IsCanceled => Status == TicketStatus.Canceled;
        public bool IsWaitingForMatch => Status == TicketStatus.WaitingForMatch;

        public MatchmakingHandler(PlayerInfo player, MatchmakingQueueConfiguration configuration)
        {
            this.Player = player;
            this.QueueConfiguration = configuration;
            
        }

        public async UniTask<CreateMatchmakingTicketResult> CreateTicket(IMatchmakingPlayerAttributes matchmakingPlayerAttributes = null)
        {
            var utcs = new UniTaskCompletionSource<CreateMatchmakingTicketResult>();

            var request = new CreateMatchmakingTicketRequest
            {
                Creator = MatchmakingHelper.GetMatchmakingPlayer(this.Player, matchmakingPlayerAttributes),
                GiveUpAfterSeconds = QueueConfiguration.GiveUpAfterSeconds,
                QueueName = QueueConfiguration.QueueName,
                AuthenticationContext = this.Player.AuthenticationContext
            };

            PlayFabMultiplayerAPI.CreateMatchmakingTicket(request, 
                result =>
                {
                    Debug.Log("매치메이킹 티켓 생성 성공");
                    this.MatchmakingTicketResult = result;
                    utcs.TrySetResult(result);
                }, 
                error =>
                {
                    var result = $"매치메이킹 실패, {error}";
                    Debug.LogError(result);
                    utcs.TrySetResult(null);
                });

            return await utcs.Task;
        }

        public async UniTask CancelPlayerTicket()
        {
            var utcs = new UniTaskCompletionSource();

            var request = new CancelAllMatchmakingTicketsForPlayerRequest
            {
                QueueName = QueueConfiguration.QueueName,
                Entity = new EntityKey
                {
                    Id = this.Player.Entity.Id,
                    Type = this.Player.Entity.Type
                },
                AuthenticationContext = this.Player.AuthenticationContext
            };

            PlayFabMultiplayerAPI.CancelAllMatchmakingTicketsForPlayer(request,
                result =>
                {
                    Debug.Log("매치메이킹 티켓 취소 성공");                    

                    this.MatchmakingTicketResult = null;
                    this.MatchmakingTicketStatus = null;

                    cancellationTokenSource?.Cancel();
                    utcs.TrySetResult();
                },
                error =>
                {
                    var result = $"매치메이킹 티켓 취소 실패, {error}";
                    Debug.LogError(result);
                    utcs.TrySetResult();
                });

            await utcs.Task;
        }

        public async UniTask<GetMatchmakingTicketResult> GetTicketStatus()
        {
            //티켓 생성 확인
            if(this.MatchmakingTicketResult == null)
            {
                var result = $"매치메이킹 티켓 생성 되지 않음.";
                Debug.LogError(result);
                return null;
            }

            var utcs = new UniTaskCompletionSource<GetMatchmakingTicketResult>();
            var request = new GetMatchmakingTicketRequest
            {
                QueueName = QueueConfiguration.QueueName,
                EscapeObject = QueueConfiguration.EscapeObject,
                TicketId = this.MatchmakingTicketResult.TicketId,
                AuthenticationContext = this.Player.AuthenticationContext
            };

            PlayFabMultiplayerAPI.GetMatchmakingTicket(request,
                result =>
                {
                    Debug.Log($"매치메이킹 상태 가져오기 => {result.Status}");

                    this.MatchmakingTicketStatus = result;
                    utcs.TrySetResult(result);
                },
                error =>
                {
                    var result = $"티켓상태 가져오기  실패, {error}";
                    Debug.LogError(result);
                    utcs.TrySetResult(null);
                });

            return await utcs.Task;
        }

        public async UniTask<GetMatchResult> GetMatch()
        {
            //티켓 생성 확인
            if (this.MatchmakingTicketResult == null)
            {
                var result = $"매치메이킹 티켓 생성 되지 않음.";
                Debug.LogError(result);
                return null;
            }

            //매치메이킹 상태 확인
            if (this.MatchmakingTicketStatus == null || !this.IsMatched)
            {
                var result = $"매치메이킹 매치 되지 않음.";
                Debug.LogError(result);
                return null;
            }


            var utcs = new UniTaskCompletionSource<GetMatchResult>();
            var request = new GetMatchRequest
            {
                EscapeObject = QueueConfiguration.EscapeObject,
                MatchId = this.MatchmakingTicketStatus.MatchId,
                QueueName = QueueConfiguration.QueueName,
                ReturnMemberAttributes = QueueConfiguration.ReturnMemberAttributes,
                AuthenticationContext = this.Player.AuthenticationContext
            };

            PlayFabMultiplayerAPI.GetMatch(request,
                result =>
                {
                    Debug.Log($"매치메이킹 매치 가져오기 성공");

                    this.MatchResult = result;
                    utcs.TrySetResult(result);
                }, 
                error =>
                {
                    var result = $"매치 가져오기 실패, {error}";
                    Debug.LogError(result);
                    utcs.TrySetResult(null);
                });           

            return await utcs.Task;
        }

        public async UniTask EnsureGetTicketStatus()
        {
            if(this.MatchmakingTicketStatus != null)
            {
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            do
            {
                await GetTicketStatus();
                if (this.IsCanceled)
                {
                    this.MatchmakingTicketStatus = null;
                }

                if (this.MatchmakingTicketStatus != null && !this.IsMatched)
                {
                    try
                    {
                        await UniTask.Delay((int)(1000 * Constants.RETRY_GET_TICKET_STATUS_AFTER_SECONDS), cancellationToken: this.cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException) { }                    
                }
            }
            while (MatchmakingTicketStatus != null
                && !this.IsMatched
                && !this.IsCanceled);

            cancellationTokenSource = null;
        }
    }
}