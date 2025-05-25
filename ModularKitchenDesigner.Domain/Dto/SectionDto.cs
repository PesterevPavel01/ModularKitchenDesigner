using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Entityes;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class SectionDto
    {
        public SectionDto() { }

        public SectionDto(Section section)
        {
            Quantity = section.Quantity;
            ModuleCode = section.Module.Code;
            KitchenCode = section.Kitchen.Code;
        }

        [Required(ErrorMessage = "Quantity cannot be null or empty.")]
        public short Quantity { get; set; }

        [Required(ErrorMessage = "ModuleCode cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "KitchenCode cannot be null or empty.")]
        public string KitchenCode { get; set; }

        public string Code { get; set; } 
    }
}
