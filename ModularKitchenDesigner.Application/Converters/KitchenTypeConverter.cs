using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
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

        public async Task<List<KitchenType>> Convert(List<KitchenTypeDto> models, List<KitchenType> entities, Func<KitchenTypeDto, Func<KitchenType, bool>> findEntityByDto, string[] validatorSuffix)
        {
            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => models.Select(model => model.PriceSegment).Contains(x.Title)),
                    preffix: "",
                    suffix: validatorSuffix);

            foreach (KitchenTypeDto model in models)
            {
                KitchenType? entity = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: entities.FirstOrDefault(findEntityByDto(model)),
                    suffix: validatorSuffix,
                    preffix: $"Элемент вызвавший ошибку: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
                );

                entity.Title = model.Title;
                entity.Code = model.Code;
                entity.PriceSegmentId = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: priceSegmentResult.Find(segment => segment.Title == model.PriceSegment),
                    suffix: validatorSuffix)?.Id ?? entity.PriceSegmentId;
            }
            return entities;
        }

    }
}
