using ModularKitchenDesigner.Domain.Interfaces;
using ModularKitchenDesigner.Domain.Interfaces.Converters;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Repository;

namespace ModularKitchenDesigner.Application.Processors
{
    public sealed class ProcessorFactory : IProcessorFactory
    {
        private Dictionary<Type, object>? _processors = [];
        private readonly IRepositoryFactory _repositoryFactory = null!;
        private readonly IValidatorFactory _validatorFactory = null!;
        private readonly IDtoToEntityConverterFactory _converterFactory = null!;

        public ProcessorFactory(IValidatorFactory validatorFactory, IRepositoryFactory repositoryFactory, IDtoToEntityConverterFactory converterFactory)
        {
            _repositoryFactory = repositoryFactory;
            _validatorFactory = validatorFactory;
            _converterFactory = converterFactory;
        }
        
        public ILoaderProcessor<TEntity, TDto> GetLoaderProcessor<TProcessor, TEntity, TDto>()
            where TEntity : class, IDtoConvertible<TEntity, TDto>
            where TDto : class
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
        
        public ICreatorProcessor<TDto, TEntity> GetCreatorProcessor<TProcessor, TEntity, TDto>()
            where TEntity : class, IDtoConvertible<TEntity, TDto>
            where TDto : class
            where TProcessor : class, ICreatorProcessor<TDto, TEntity>, new()
        {
            var type = typeof(TProcessor);

            if (!_processors.ContainsKey(type))
            {
                _processors[type] = new TProcessor()
                    .SetValidatorFactory(_validatorFactory)
                    .SetRepositoryFactory(_repositoryFactory)
                    .SetDtoToEntityConverterFactory(_converterFactory);
            }

            return (ICreatorProcessor<TDto, TEntity>)_processors[type];
        }
    }
}
