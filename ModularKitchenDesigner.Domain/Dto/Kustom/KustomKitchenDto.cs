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

        public List<SpecificationItem> Specification { get; set; } = [];
    }
}
