using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    public class SimpleEntityCreatorProcessor<TEntity> : ICreatorProcessor <SimpleDto, BaseResult<SimpleDto>>
        where TEntity : class, ISimpleEntity, new()
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<SimpleDto, BaseResult<SimpleDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public ICreatorProcessor<SimpleDto, BaseResult<SimpleDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public async Task<BaseResult<SimpleDto>> ProcessAsync(SimpleDto model)
        {
            if (model is null) throw new ArgumentNullException(typeof(TEntity).Name);

            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}"
            ];

            var checkCodeResult = _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(predicate: x => x.Code == model.Code),
                    preffix: "",
                    suffix: suffix);

            var result = await _repositoryFactory.GetRepository<TEntity>().CreateAsync(new() { Code = model.Code, Title = model.Title });

            return new()
            {
                Data = new SimpleDto() { Code = result.Code, Title = result.Title },
                ConnectionTime = DateTime.Now
            };
        }
    }
}
