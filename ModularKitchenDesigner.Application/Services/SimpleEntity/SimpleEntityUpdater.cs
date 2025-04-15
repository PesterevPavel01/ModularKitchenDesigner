using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.SimpleEntity;
using Newtonsoft.Json;
using Repository;
using Result;
using System.Text;

namespace ModularKitchenDesigner.Application.Services.SimpleEntity
{
    internal class SimpleEntityUpdater<TEntity, TDto> : ISimpleEntityUpdater<TEntity, TDto>
        where TEntity : class, ISimpleEntity, new()
        where TDto : class, ISimpleEntity, new()
    {
        public SimpleEntityUpdater(IRepositoryFactory repositoryFactory)
        {
            _repository = repositoryFactory.GetRepository<TEntity>();
        }

        private readonly IBaseRepository<TEntity> _repository;

        public async Task<BaseResult<TDto>> UpdateAsync(TDto model)
        {
            {
                if (model is null) throw new ArgumentNullException(typeof(TEntity).Name);

                var checkCodeResult = await _repository.GetAllAsync(predicate: x => x.Code == model.Code);

                if (!checkCodeResult.Any())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"Сущность: {typeof(TEntity).Name}");
                    stringBuilder.AppendLine("Запись с таким кодом не найдена");
                    throw new Exception(stringBuilder.ToString());
                }

                var currentMaterial = checkCodeResult.First();

                currentMaterial.Title = model.Title;

                var result = await _repository.UpdateAsync(currentMaterial);

                return new()
                {
                    Data = new() { Code = result.Code, Title = result.Title },
                    ConnectionTime = DateTime.Now
                };
            }
        }
    }
}
