using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Interpritators
{
    public class ModelItemInterpreter
    {
        private IRepositoryFactory _repositoryFactory = null!;

        public ModelItemInterpreter(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<CollectionResult<NomanclatureDto>> InterpretAsync(List<NomanclatureDto> externalModels)
        {
            if (externalModels is null || externalModels.Count < 1)
                return new();

            var result = externalModels
                .Where(x => x.Parents
                    .FindIndex(x => x.Code == "00080202189") == 1 && x.Models is not null)
                .SelectMany(x => x.Models
                    .Select(model =>
                        new NomanclatureDto()
                        {
                            Code = x.Code,
                            Models = [model],
                            Parents = x.Parents,
                            Title = model.Title != "removed" ? x.Title : "removed"
                        }))
                .ToList();

            var existingModels = await _repositoryFactory.GetRepository<ModelItem>().GetAllAsync(
                include: ModelItem.IncludeRequaredField(),
                predicate: x => externalModels.Select(model => model.Code).Contains(x.Module.Code));

            // если в Models не будет какой-то модели, которая в системе привязана к модулю через ModelItem, то существующая модель будет включена в входной пакет моделей с пометкой удаления "removed" 
            // для установки у нее флага Enabled = false при дальнейшем переносе данных

            if (existingModels.Count > 0)
                result.AddRange(existingModels
                    .Where(model => result.FirstOrDefault(x => x.Code == model.Module.Code && x.Models.Count > 0 && x.Models[0].Code == model.Model.Code) is null)
                    .Select(model => new NomanclatureDto
                    {
                        Title = model.Module.Title,
                        Code = model.Module.Code,
                        Models =
                        [
                            new()
                            { 
                                Code = model.Model.Code,
                                Title = "removed",
                            }
                        ],
                        Parents = 
                        [
                                new(),
                                new()
                                {
                                    Code = "00080202189"
                                }
                        ]
                    }).ToList());

            return
            new()
            {
                Count = result.Count,
                Data = result
            };
        }
    }
}
