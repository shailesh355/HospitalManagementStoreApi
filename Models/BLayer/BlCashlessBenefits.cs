namespace TicketManagementApi.Models.BLayer
{
    public class BlCashlessBenefits
    {
        public Int16 CRUD { get; set; }
        public long? hospitalRegNo { get; set; }
        public Int64? userId { get; set; }
        public string? entryDateTime { get; set; }
        public string? clientIp { get; set; }
        public List<BlCashlessBenefitsItems>? Bl { get; set; }
    }
    public class BlCashlessBenefitsItems
    {
        public Int32 cashlessBenefitsId { get; set; }
        public Int16 discountPercent { get; set; }
    }
}
