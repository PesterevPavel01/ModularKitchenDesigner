using System.ComponentModel.DataAnnotations;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class SectionDto : PrivateIdentity
    {
        public SectionDto() { }

        public SectionDto(Section section)
        {
            Quantity = section.Quantity;
            ModuleCode = section.Module.Code;
            KitchenGuid = section.Kitchen.Id;
        }

        [Required(ErrorMessage = "Quantity cannot be null or empty.")]
        public short Quantity { get; set; }

        [Required(ErrorMessage = "ModuleCode cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "KitchenGuid cannot be null or empty.")]
        public Guid KitchenGuid { get; set; }
    }
}
