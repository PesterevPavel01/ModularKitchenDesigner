using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
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

        public async Task<List<KitchenType>> Convert(List<KitchenTypeDto> models, List<KitchenType> entities)
        {
            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => models.Select(model => model.PriceSegment).Contains(x.Title)),
                    methodArgument: models,
                    callerObject: GetType().Name);

            List<KitchenType> kitchens = [];

            foreach (KitchenTypeDto model in models)
            {
                var title = model.Title;
                var code = model.Code;
                var priceSegment = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: priceSegmentResult.Find(segment => segment.Title == model.PriceSegment),
                    methodArgument: models,
                    callerObject: GetType().Name);

                KitchenType? entity = entities.Find(x => x.IsUniqueKeyEqual(model));

                if (entity is null)
                    kitchens.Add(
                        KitchenType.Create(
                            title: title,
                            code: code,
                            priceSegment: priceSegment
                            ));
                else
                    kitchens.Add(
                        entity.Update(
                            title: title,
                            code: code,
                            priceSegment: priceSegment));
            }
            return kitchens;
        }

    }
}
