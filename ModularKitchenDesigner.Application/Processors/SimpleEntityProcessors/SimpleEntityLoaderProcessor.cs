using System.Linq.Expressions;
using Interceptors;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces.Base;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Processors.SimpleEntityProcessors
{
    public class SimpleEntityLoaderProcessor<TEntity> : ILoaderProcessor<TEntity, SimpleDto>
        where TEntity : Identity, IAuditable, ISimpleEntity
    {
        private IRepositoryFactory _repositoryFactory = null!;
        private IValidatorFactory _validatorFactory = null!;

        public ILoaderProcessor<TEntity, SimpleDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public ILoaderProcessor<TEntity, SimpleDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }
        
        public async Task<CollectionResult<SimpleDto>> ProcessAsync(Expression<Func<TEntity, bool>> ? predicate = null)
        {
            string[] suffix = [
                $"Object: {GetType().Name}",
                $"Argument: {predicate?.GetType().Name}"
            ];

            var result = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<TEntity>().GetAllAsync(predicate: predicate),
                    methodArgument: predicate.GetType().Name,
                    callerObject: GetType().Name);

            var resultList = result.Select(x => new SimpleDto() { Code = x.Code, Title = x.Title }).ToList();

            return new()
            {
                Data = resultList,
                Count = resultList.Count,
                ConnectionTime = DateTime.Now
            };
        }

        /*
        public async Task<BaseResult<TDto>> GetByUniquePropertyAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var result = await _repository.GetAllAsync(trackingType: TrackingType.NoTracking, predicate: predicate);

            if (!result.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Сущность: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("Запись с таким кодом не найдена");
                throw new Exception(stringBuilder.ToString());
            }

            if (result.Count > 1)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Сущность: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("Существует более одной записи соответствующей заданным условиям!");
                result.Select(x => stringBuilder.AppendLine(JsonConvert.SerializeObject(x, Formatting.Indented)));
                throw new Exception(stringBuilder.ToString());
            }

            var resultModel = result.Select(x => new TDto() { Code = x.Code, Title = x.Title }).First();

            return new BaseResult<TDto>()
            {
                Data = resultModel,
                ConnectionTime = DateTime.Now
            };
        }
        */
    }
}
