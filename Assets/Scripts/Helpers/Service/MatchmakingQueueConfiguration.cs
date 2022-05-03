namespace PlayFabStudy.Helpers
{
    public class MatchmakingQueueConfiguration
    {
        public string QueueName { get; set; }
        public int GiveUpAfterSeconds { get; set; } = Constants.GIVE_UP_AFTER_SECONDS;
        public bool EscapeObject { get; set; }
        public bool ReturnMemberAttributes { get; set; }
    } 
}
