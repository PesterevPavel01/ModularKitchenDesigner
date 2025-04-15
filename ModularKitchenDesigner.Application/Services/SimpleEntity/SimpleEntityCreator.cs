using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;
using Newtonsoft.Json;
using Repository;
using Result;
using System.Text;

namespace ModularKitchenDesigner.Application.Services.SimpleEntity
{
    internal class SimpleEntityCreator<TEntity, TDto> : ISimpleEntityCreator <TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        public SimpleEntityCreator(IRepositoryFactory repositoryFactory)
        {
            _materialRepository = repositoryFactory.GetRepository<TEntity>();
        }

        private readonly IBaseRepository<TEntity> _materialRepository;

        public async Task<BaseResult<TDto>> CreateAsync(TDto model)
        {
            if (model is null) throw new ArgumentNullException(typeof(TEntity).Name);

            var checkCodeResult = await _materialRepository.GetAllAsync(predicate: x => x.Code == model.Code, trackingType : TrackingType.NoTracking);

            if (checkCodeResult.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Сущность: {typeof(TEntity).Name}");
                stringBuilder.AppendLine("Запись с таким кодом уже существует");
                checkCodeResult.Select(x => stringBuilder.AppendLine(JsonConvert.SerializeObject(x, Formatting.Indented)));
                throw new Exception(stringBuilder.ToString());
            }

            var result = await _materialRepository.CreateAsync(new() { Code = model.Code, Title = model.Title });

            return new()
            {
                Data = new TDto() { Code = result.Code, Title = result.Title },
                ConnectionTime = DateTime.Now
            };
        }
    }
}
