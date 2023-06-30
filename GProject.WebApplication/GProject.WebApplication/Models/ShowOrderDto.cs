using GProject.Data.DomainClass;

namespace GProject.WebApplication.Models
{
    public class ShowOrderDto
    {
        public Cart Cart { get; set; }
        public Brand Brand { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public Product Product { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
