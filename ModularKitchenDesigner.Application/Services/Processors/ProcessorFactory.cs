using Interceptors;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Services.Processors
{
    internal class ProcessorFactory<TEntity, TDto> : IProcessorFactory<TEntity, TDto>
        where TEntity : Identity, IAuditable
        where TDto : class
    {
        private Dictionary<Type, object>? _processors = [];
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IValidatorFactory _validatorFactory;

        public ProcessorFactory(IValidatorFactory validatorFactory, IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _validatorFactory = validatorFactory;
        }

        public ICreatorProcessor<TDto> GetCreatorProcessor<TProcessor>()
            where TProcessor : class, ICreatorProcessor<TDto>, new()
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (ICreatorProcessor<TDto>)_processors[type];
        }

        public ILoaderProcessor<TEntity, TDto> GetLoaderProcessor<TProcessor>()
            where TProcessor : class, ILoaderProcessor<TEntity, TDto>, new()
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory);
            }

            return (ILoaderProcessor<TEntity, TDto>)_processors[type];
        }
    }
}
