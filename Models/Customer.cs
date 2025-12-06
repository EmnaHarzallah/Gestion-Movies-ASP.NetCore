namespace AspCoreFirstApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MembershipName { get; set; }
        public int? DiscountRate { get; set; }
    }
}