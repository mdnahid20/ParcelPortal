namespace ParcelPortal.Models
{
    public class Courier
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public string? ConsignmentNumber { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set;}
        public string SenderPhone { get; set;} 
        public string SenderBranch { get; set; }
        public string SenderAddress {  get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverBranch { get; set; }
        public string ReceiverAddress { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductPrice { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Status {  get; set; } 
    }
}
