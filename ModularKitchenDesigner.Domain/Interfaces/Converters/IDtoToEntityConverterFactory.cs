namespace ModularKitchenDesigner.Domain.Interfaces.Converters
{
    public interface IDtoToEntityConverterFactory
    {
        IDtoToEntityConverter<TEntity, TDto> GetConverter<TEntity, TDto, TConverter>()
            where TEntity : class, IDtoConvertible<TEntity, TDto>
            where TConverter : IDtoToEntityConverter<TEntity, TDto>, new();
    }
}
