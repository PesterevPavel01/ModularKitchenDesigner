using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;
using Result;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ModularKitchenDesigner.Application.Services.Processors.ModuleProcessor.ModuleLoaders
{
    public sealed class DefaultModuleLoaderProcessor : ILoaderProcessor<Module, ModuleDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;


        public ILoaderProcessor<Module, ModuleDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ILoaderProcessor<Module, ModuleDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }
        public async Task<CollectionResult<ModuleDto>> ProcessAsync(Expression<Func<Module, bool>> predicate = null)
        {
            List<Module> models = _validatorFactory
                .GetEmptyListValidator()
                .Validate(
                    models: await _repositoryFactory.GetRepository<Module>().GetAllAsync(
                        include: query => query.Include(x => x.Type),
                        predicate: predicate),
                    preffix: "",
                    suffix: "Object: DefaultModuleLoaderProcessor.ProcessAsync(Expression<Func<Module, bool>> predicate)");

            return new()
            {
                Count = models.Count,
                Data = models.Select(x => new ModuleDto(x))
            };
        }
    }
}
