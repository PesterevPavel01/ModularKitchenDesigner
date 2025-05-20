using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    public class SimleEntityMultipleCreatorProcessor<TEntity> : ICreatorProcessor<List<SimpleDto>, CollectionResult<SimpleDto>>
                where TEntity : class, ISimpleEntity, new()
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ICreatorProcessor<List<SimpleDto>, CollectionResult<SimpleDto>> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<List<SimpleDto>, CollectionResult<SimpleDto>> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<SimpleDto>> ProcessAsync(List<SimpleDto> data)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {JsonConvert.SerializeObject(data, Formatting.Indented)}"
            ];

            _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: data,
                    suffix: suffix
                );

            var checkCodeResult = _validatorFactory
            .GetCreateValidator()
            .Validate(
                    models: await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(predicate: x => data.Select(model => model.Code).Contains(x.Code)),
            preffix: "",
            suffix: suffix);

            var result = await _repositoryFactory
                .GetRepository<TEntity>()
                .CreateMultipleAsync(
                data.Select( model => new TEntity()
                { 
                    Code = model.Code,
                    Title = model.Title 
                }).ToList());

            return new()
            {
                Count = result.Count,
                Data = result
                    .Select( model => new SimpleDto() 
                    { 
                        Code = model.Code,
                        Title = model.Title 
                    }),
                ConnectionTime = DateTime.Now
            };
        }
    }
}
