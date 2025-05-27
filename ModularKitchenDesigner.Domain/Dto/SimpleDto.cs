using ModularKitchenDesigner.Domain.Dto.Base;
using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Interfaces.Exchange;

namespace ModularKitchenDesigner.Domain.Dto
{
    public sealed class SimpleDto : BaseDto, IExcangeDtoConvertable<SimpleDto,NomanclatureDto>
    {
        public SimpleDto() { }
        public SimpleDto(string title, string code)
            : base(title: title, code: code) { }
        public SimpleDto Convert(NomanclatureDto dto)
        {
            Code = dto.Code;
            Title = dto.Title;

            return this;
        }
    }
}