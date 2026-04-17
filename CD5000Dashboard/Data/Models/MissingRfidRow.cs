namespace CD5000Dashboard.Data.Models
{
    public class MissingRfidRow
    {
        public long Pk { get; set; }
        public string? Vehicle { get; set; }
        public string? TagId { get; set; }
        public string? UnitTrain { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
    }
}
