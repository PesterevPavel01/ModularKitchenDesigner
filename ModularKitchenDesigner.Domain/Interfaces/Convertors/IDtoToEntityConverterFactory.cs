using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;

namespace ModularKitchenDesigner.Domain.Interfaces.Convertors
{
    public interface IDtoToEntityConverterFactory
    {
        IDtoToEntityConverter<TEntity, TDto> GetConverter<TEntity, TDto, TConverter>()
            where TEntity : class, IConvertibleToDto<TEntity, TDto>, new()
            where TConverter : IDtoToEntityConverter<TEntity, TDto>, new();
    }
}
