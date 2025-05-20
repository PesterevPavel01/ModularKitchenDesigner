using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntity;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    public class SimpleEntityRemoveProcessor<TEntity> : ISimpleEntityRemoveProcessor
        where TEntity : class, ISimpleEntity, new()
    {

        public SimpleEntityRemoveProcessor(IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            _repository = repositoryFactory.GetRepository<TEntity>();
        }

        private IValidatorFactory _validatorFactory = null!;
        private readonly IBaseRepository<TEntity> _repository;

        public async Task<Result.BaseResult<SimpleDto>> RemoveAsync(string code)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(code, Formatting.Indented)}"
            ];

            var currentRecord = _validatorFactory
               .GetObjectNullValidator()
               .Validate(
                   model: (await _repository.GetAllAsync(predicate: x => x.Code == code, trackingType: TrackingType.Tracking)).FirstOrDefault(),
                   preffix: "",
                   suffix: suffix);

            var result = await _repository.RemoveAsync(currentRecord);

            return new()
            {
                Data = new SimpleDto() { Code = currentRecord.Code, Title = currentRecord.Title },
                ConnectionTime = DateTime.Now
            };
        }
    }
}
