using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.BlockProcessor.BlockCreator
{
    public sealed class SingleBlockCreatorProcessor : ICreatorProcessor<BlockDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;
        public ICreatorProcessor<BlockDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<BlockDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<BlockDto>> ProcessAsync(BlockDto model)
        {
            var blockResult = await _repositoryFactory.GetRepository<Block>().GetAllAsync(predicate: x => x.Component.Code == model.ComponentCode && x.Module.Code == model.ModuleCode);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: blockResult,
                    preffix: "",
                    $"Object: SingleBlockCreatorProcessor.CreateAsync(BlockDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var componentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Component>().GetAllAsync(predicate: x => x.Code == model.ComponentCode)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleBlockCreatorProcessor.CreateAsync(BlockDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var modulResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<Module>().GetAllAsync(predicate: x => x.Code == model.ModuleCode)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleBlockCreatorProcessor.CreateAsync(BlockDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            Block componentCreatorResult = await _repositoryFactory
                .GetRepository<Block>()
                .CreateAsync(
                    new Block()
                    {
                        ComponentId = componentResult.Id,
                        ModuleId = modulResult.Id,
                        Quantity = model.Quanyity
                    });

            var newBlock = (await _repositoryFactory
                .GetRepository<Block>()
                .GetAllAsync(
                    include: query => query.Include(x => x.Component).Include(x => x.Module),
                    predicate: x => x.Module.Code == model.ModuleCode && x.Component.Code == model.ComponentCode
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newBlock)
            };
        }

    }
}
