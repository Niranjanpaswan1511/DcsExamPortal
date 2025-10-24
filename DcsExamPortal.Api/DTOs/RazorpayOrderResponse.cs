namespace DcsExamPortal.Api.DTOs
{
    public class RazorpayOrderResponse
    {
        public string Key { get; set; }
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
