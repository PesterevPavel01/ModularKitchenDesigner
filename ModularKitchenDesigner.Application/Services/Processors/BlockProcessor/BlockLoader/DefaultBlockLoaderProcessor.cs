using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.BlockProcessor.BlockLoader
{
    public sealed class DefaultBlockLoaderProcessor : ILoaderProcessor<Block, BlockDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ILoaderProcessor<Block, BlockDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<Block, BlockDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<CollectionResult<BlockDto>> ProcessAsync(Expression<Func<Block, bool>>? predicate = null)
        {
            List<Block> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Block>().GetAllAsync(
                        include: query => query.Include(x => x.Component).Include(x => x.Module),
                        predicate: predicate),
                    preffix: "",
                    suffix: "Object: DefaultBlockLoaderProcessor.ProcessAsync(Expression<Func<Block, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new BlockDto(x))
            };
        }
    }
}