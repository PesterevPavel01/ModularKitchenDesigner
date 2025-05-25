using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Domain.Interfaces.Converters
{
    public interface IDtoToEntityConverter<TEntity, TDto>
    {
        /// <summary>
        /// Функция для обновления коллекции сущностей entities до состояния соответствующей модели из коллекции models
        /// </summary>
        /// <param name="models">коллекция целевых моделей</param>
        /// <param name="entities">коллекция сущностей</param>
        /// <param name="validatorSuffix">суффикс для сообщения об ошибке</param>
        /// <returns></returns>
        Task<List<TEntity>> Convert(List<TDto> models, List<TEntity> entities);
        IDtoToEntityConverter<TEntity, TDto> SetValidatorFactory(IValidatorFactory validatorFactory);
        IDtoToEntityConverter<TEntity, TDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory);
    }
}
