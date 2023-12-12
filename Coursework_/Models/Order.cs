namespace Coursework_.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public DateTime OrderTime { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
