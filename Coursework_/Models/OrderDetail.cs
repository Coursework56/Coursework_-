using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework_.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public virtual Product product { get; set; }

        public virtual Order Order { get; set; }

    }
}
