namespace AdministartionConsole.Models.ViewModel
{
    public class ProductViewModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int AvailableAmount { get; set; }
        public string Status { get; set; }
        public string Customer { get; set; } // Username
        public string Category { get; set; }
    }
}
