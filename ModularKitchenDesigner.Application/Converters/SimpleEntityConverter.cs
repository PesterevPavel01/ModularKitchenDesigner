using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
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

        public async Task<List<TEntity>> Convert(List<SimpleDto> models, List<TEntity> entities, Func<SimpleDto, Func<TEntity, bool>> findEntityByDto)
        {
            foreach (SimpleDto model in models) 
            {
                TEntity? entity  = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.Title = model?.Title;
            }
           
            return entities;
        }
    }
}
