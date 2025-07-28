using ModularKitchenDesigner.Domain.Dto.Exchange;
using ModularKitchenDesigner.Domain.Entityes;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Exchange.Interpritators
{
    public class MaterialSelectionItemInterpreter
    {
        private IRepositoryFactory _repositoryFactory = null!;

        public MaterialSelectionItemInterpreter(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<CollectionResult<NomanclatureDto>> InterpretAsync(List<NomanclatureDto> externalModels)
        {
            if (externalModels is null || externalModels.Count < 1)
                return new();

            var existingModels =  await _repositoryFactory.GetRepository<MaterialSelectionItem>().GetAllAsync(
                include: MaterialSelectionItem.IncludeRequaredField(),
                predicate: x => externalModels.Select(model => model.Code).Contains(x.Material.Code));

            List<NomanclatureDto> result = [];
            List<MaterialSelectionItem> removedModels = [];

            if (existingModels.Any())
            {
                removedModels.AddRange([.. existingModels.Where(model => externalModels.FirstOrDefault(x => x.Code == model.Material.Code)?.Parents.FindIndex(parent => parent.Code == model.KitchenType.Code) != 0)]);
                removedModels.AddRange([.. existingModels.Where(model => externalModels.Where(x => x.Title == "removed").Select(x => x.Code).Contains(model.Material.Code))]);
            }

            if (removedModels.Count > 0)
            {     //может быть ситуация, при которой у externalModel нет элемента Parents[0]

                result.AddRange([.. removedModels
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
                            new(),
                            new()
                            {
                                Title = model.ComponentType.Title,
                                Code = model.ComponentType.Code
                            }
                        ]

                    })]);
            }

            result.AddRange(externalModels.Where(model => !removedModels.Select(x => x.Material.Code).Contains(model.Code)));

            return
                new() 
                {
                    Count = result.Count,
                    Data = result
                };
        }
    }
}
