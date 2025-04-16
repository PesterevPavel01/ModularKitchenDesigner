using ModularKitchenDesigner.Domain.Entityes;
using System.ComponentModel.DataAnnotations;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class BlockDto
    {
        public BlockDto() { }

        public BlockDto(Block block) 
        {
            Quanyity = block.Quantity;
            ModuleCode = block.Module.Code;
            ComponentCode = block.Component.Code;
        }

        public short Quanyity { get; set; }

        [Required(ErrorMessage = "Module cannot be null or empty.")]
        public string ModuleCode { get; set; }

        [Required(ErrorMessage = "Component cannot be null or empty.")]
        public string ComponentCode { get; set; }
    }
}
