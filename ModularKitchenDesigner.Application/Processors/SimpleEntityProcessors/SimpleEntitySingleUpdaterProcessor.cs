using System.Linq.Expressions;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Convertors;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    internal class SimpleEntitySingleUpdaterProcessor<TEntity, TConverter>: IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, TEntity>
        where TEntity : class, ISimpleEntity, IConvertibleToDto<TEntity, SimpleDto>, new()
        where TConverter : IDtoToEntityConverter<TEntity, SimpleDto>, new()
    {

        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;
        private IDtoToEntityConverterFactory _converterFactory = null!;

        public IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, TEntity> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, TEntity> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }
        public IUpdaterProcessor<SimpleDto, BaseResult<SimpleDto>, TEntity> SetDtoToEntityConverterFactory(IDtoToEntityConverterFactory converterFactory)
        {
            _converterFactory = converterFactory;
            return this;
        }

        public async Task<BaseResult<SimpleDto>> ProcessAsync(SimpleDto model, Expression<Func<TEntity, bool>>? predicate = null)
        {
            _validatorFactory
                .GetObjectNullValidator()
                .Validate(model);

            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var currentEntity =_validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(
                        predicate: x => x.Code == model.Code,
                        trackingType: TrackingType.Tracking)).FirstOrDefault(),
                    suffix: suffix
                );

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .UpdateMultipleAsync(await _converterFactory.GetConverter<TEntity, SimpleDto, TConverter>().Convert([model], [currentEntity], suffix));

            return new()
            {
                Data = new() { Code = result.First().Code, Title = result.First().Title },
                ConnectionTime = DateTime.Now
            };
        }

    }
}
