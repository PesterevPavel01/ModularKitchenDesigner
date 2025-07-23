using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto.Kustom
{
    public sealed class KustomKitchenDto
    {
        public string KitchenTitle { get; set; }

        [Required(ErrorMessage = "KitchenCode cannot be null or empty.")]
        public string KitchenCode { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public double Width { get; set; }
        public double Price { get; set; }

        public List<ModuleSpecification> ModuleSpecifications { get; set; } = [];


        public class ModuleSpecification
        {
            public string Type { get; set; }
            public string Code { get; set; }
            public string Title { get; set; }
            public double Quantity { get; set; }
            public double Price { get; set; }
            public double TotalPrice { get; set; }
            public List<SpecificationItem> Specification { get; set; } = [];
        }
    }
}
