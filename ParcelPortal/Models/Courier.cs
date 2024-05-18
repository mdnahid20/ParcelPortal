namespace ParcelPortal.Models
{
    public class Courier
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public string SenderName { get; set; }
        public string SenderEmail { get; set;}
        public string SenderPhone { get; set;} 
        public string SenderBranch { get; set; }
        public string SenderAddress {  get; set; }
        public string RecevierName { get; set; }
        public string RecevierEmail { get; set; }
        public string RecevierPhone { get; set; }
        public string ReceiverBranch { get; set; }
        public string RecevierAddress { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductPrice { get; set; }
        public string? DeliveryTime { get; set; }
    }
}
