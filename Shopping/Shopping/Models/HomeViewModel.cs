using Shopping.Entities;

namespace Shopping.Models
{
    public class HomeViewModel
    {
        public float Quantity { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
