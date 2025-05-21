using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class SimpleEntityConverter<TEntity> : IDtoToEntityConverter<TEntity, SimpleDto>
        where TEntity : Identity, ISimpleEntity, new()
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        public IDtoToEntityConverter<TEntity, SimpleDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<TEntity, SimpleDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<TEntity>> Convert(List<SimpleDto> models, List<TEntity> entities, Func<SimpleDto, Func<TEntity, bool>> findEntityByDto, string[] validatorSuffix)
        {
            foreach (SimpleDto model in models) 
            {
                TEntity? entity  = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.Title = model?.Title;
            }
           
            return entities;
        }
    }
}
