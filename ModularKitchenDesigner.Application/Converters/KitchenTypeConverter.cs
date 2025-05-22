using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Converters
{
    public sealed class KitchenTypeConverter : IDtoToEntityConverter<KitchenType, KitchenTypeDto>
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public IDtoToEntityConverter<KitchenType, KitchenTypeDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public IDtoToEntityConverter<KitchenType, KitchenTypeDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<List<KitchenType>> Convert(List<KitchenTypeDto> models, List<KitchenType> entities, Func<KitchenTypeDto, Func<KitchenType, bool>> findEntityByDto)
        {
            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => models.Select(model => model.PriceSegment).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            foreach (KitchenTypeDto model in models)
            {
                KitchenType? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    methodArgument: models,
                    callerObject: GetType().Name);

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.PriceSegmentId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: priceSegmentResult.Find(segment => segment.Title == model.PriceSegment),
                    methodArgument: models,
                    callerObject: GetType().Name)?.Id ?? entity.PriceSegmentId;
            }
            return entities;
        }

    }
}
