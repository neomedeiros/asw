namespace ASW.Entities
{
    /// <summary>
    /// Data entity to store information for Diff Requests
    /// </summary>
    public class ComparisonRequestEntity
    {
        public long Id { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}