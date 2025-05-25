using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class SimpleEntityConverter<TEntity> : IDtoToEntityConverter<TEntity, SimpleDto>
        where TEntity : Identity, ISimpleEntity, IDtoConvertible<TEntity, SimpleDto>, new()
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

        public async Task<List<TEntity>> Convert(List<SimpleDto> models, List<TEntity> entities)
        {
            List<TEntity> simpleEntities = [];

            foreach (SimpleDto model in models) 
            {

                TEntity? entity  = entities.Find(entity => entity.isUniqueKeyEqual(model));

                if (entity is null) 
                {
                    entity = new TEntity();
                    entity.Title = model?.Title;
                    entity.Code = model?.Code;
                }
                else 
                    entity.Title = model?.Title;

                simpleEntities.Add(entity);
            }
           
            return simpleEntities;
        }
    }
}
