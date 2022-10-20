using Shopping.Common;
using Shopping.Entities;

namespace Shopping.Models
{
    public class HomeViewModel
    {
        public float Quantity { get; set; }
        public PaginatedList<Product> Products { get; set; }
        public ICollection<Category> Categories { get; set; }

    }
}
