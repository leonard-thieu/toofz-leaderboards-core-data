using System.Runtime.Serialization;

namespace toofz.NecroDancer.Leaderboards
{
    [DataContract]
    public sealed class LeaderboardHeadersEnvelope
    {
        [DataMember(Name = "leaderboards", IsRequired = true)]
        public LeaderboardHeaders Leaderboards { get; set; }
    }
}
