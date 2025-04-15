using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;
using Newtonsoft.Json;
using Repository;
using Result;
using System.Linq.Expressions;
using System.Text;

namespace ModularKitchenDesigner.Application.Services.SimpleEntity
{
    public class SimpleEntityLoader<TEntity, TDto> : ISimpleEntityLoader<TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        public SimpleEntityLoader(IRepositoryFactory repositoryFactory)
        {
            _repository = repositoryFactory.GetRepository<TEntity>();
        }

        private readonly IBaseRepository<TEntity> _repository;

        public async Task<CollectionResult<TDto>> GetAll()
        {
            var result = await _repository.GetAllAsync(trackingType: TrackingType.NoTracking);

            if (!result.Any())
                throw new ArgumentOutOfRangeException(typeof(TEntity).Name);

            var resulttList = result.Select(x => new TDto() { Code = x.Code, Title = x.Title }).ToList();

            return new CollectionResult<TDto>()
            {
                Data = resulttList,
                Count = resulttList.Count,
                ConnectionTime = DateTime.Now
            };
        }

        public async Task<BaseResult<TDto>> GetByUniquePropertyAsync( Expression<Func<TEntity, bool>>? predicate = null)
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
    }
}
