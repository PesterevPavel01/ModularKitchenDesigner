namespace ModularKitchenDesigner.Domain.Interfaces.Exchange
{
    public interface IExcangeDtoConvertable<TDto,TExchangeDto>
    {
        public TDto Convert(TExchangeDto dto);
    }
}
