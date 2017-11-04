namespace toofz.NecroDancer.Leaderboards
{
    public sealed class BulkUpsertOptions
    {
        /// <summary>
        /// If true, updates rows that match; otherwise, only perform inserts for rows that do not match. 
        /// The default is true.
        /// </summary>
        public bool UpdateWhenMatched { get; set; } = true;
    }
}
