using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Interpritators
{
    public class MaterialSelectionItemInterpritator
    {
        private IRepositoryFactory _repositoryFactory = null!;

        public MaterialSelectionItemInterpritator(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<CollectionResult<NomanclatureDto>> MapAsync(List<NomanclatureDto> ExternalModels)
        {
            if (ExternalModels is null || ExternalModels.Count < 1)
                return new();

            var existingModels =  await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(
                        include: MaterialSelectionItem.IncludeRequaredField(),
                        predicate: x => ExternalModels.Select(model => model.Code).Contains(x.Material.Code));

            List<NomanclatureDto> result = [];
            result.AddRange(ExternalModels);

            if (existingModels.Count > 0)
                result.AddRange(existingModels.Where(model => model.KitchenType.Code != ExternalModels.FirstOrDefault(x => x.Code == model.Material.Code)?.Parents[0].Code)
                    .Select(model => new NomanclatureDto
                    {
                        Title = model.Material.Title,
                        Code = "removed",
                        Parents =
                        [
                            new()
                            {
                                Title = model.KitchenType.Title,
                                Code = model.KitchenType.Code
                            },
                            ExternalModels.FirstOrDefault(x => x.Code == model.Material.Code)?.Parents[1],
                            new()
                            {
                                Title = model.ComponentType.Title,
                                Code = model.ComponentType.Code
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
