using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Processors.SimpleEntityProcessors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    public class SimpleEntityRemoveProcessor<TEntity> : ISimpleEntityRemoveProcessor
        where TEntity : SimpleEntity, ISimpleEntity, new()
    {

        public SimpleEntityRemoveProcessor(IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            _repository = repositoryFactory.GetRepository<TEntity>();
        }

        private IValidatorFactory _validatorFactory = null!;
        private readonly IBaseRepository<TEntity> _repository;

        public async Task<BaseResult<SimpleDto>> RemoveAsync(string code)
        {
            var currentRecord = _validatorFactory
               .GetObjectNullValidator()
               .Validate(
                    model: (await _repository.GetAllAsync(predicate: x => x.Code == code, trackingType: TrackingType.Tracking)).FirstOrDefault(),
                    methodArgument: $"Code: {code}",
                    callerObject: GetType().Name);

            var result = await _repository.RemoveAsync(currentRecord);

            return new()
            {
                Data = new SimpleDto(currentRecord.Code, currentRecord.Title),
                ConnectionTime = DateTime.Now
            };
        }
    }
}
